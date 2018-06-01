using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;

namespace Images.Collect.Models
{
    public class BuildDb : IDisposable
    {
        private string defaultDirectory;
        private string[] extensionList;

        public ImageDbContext Context { get; }

        public BuildDb()
        {
            defaultDirectory = WebConfigurationManager.AppSettings.GetValues("DefaultDirectory")[0];
            extensionList = WebConfigurationManager.AppSettings.GetValues("ExtensionList")[0].Split(';');
            Context = new ImageDbContext();
        }

        /// <summary>
        ///     The interface made me do it.
        /// </summary>
        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
        }

        /// <summary>
        ///     Find and save information on all image files under the directory.
        ///     May be called recursively to process subdirectories.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns>Number of files added to database</returns>
        public int hashDirectory(string directory)
        {
            ImageData imageResults = new ImageData();
            int count = 0;
            try
            {
                string[] fileList = Directory.GetFiles(directory);
                foreach (var file in fileList)
                {
                    var fi = new FileInfo(file);
                    var ext = Path.GetExtension(file).ToLower().Substring(1);

                    if (!extensionList.Contains(ext) || IsFileInDb(fi.FullName.Trim()))
                        continue;

                    imageResults = GetHashString(file);
                    var image = Context.Images.Create();
                    image.BaseName = fi.Name.Trim();
                    image.FullName = fi.FullName.Trim();
                    image.Path = fi.DirectoryName.Trim();
                    image.FileSize = fi.Length;
                    image.FileDate = fi.LastWriteTimeUtc;
                    image.Hash = imageResults.hash;
                    image.Extension = ext;
                    image.Drive = fi.FullName.Substring(0, 1);      // get drive letter
                    image.Unreadable = imageResults.unreadable;
                    Context.Images.Add(image);
                    ++count;
                }
                Context.SaveChanges();
            }
            catch (Exception ex)
            {

            }

            string[] dirList = Directory.GetDirectories(directory);
            foreach (var dir in dirList)
            {
                count += hashDirectory(dir);
            }

            return count;
        }

        internal void PurgeOldRecords(string path)
        {
            var images = Context.Images.Where(i => i.Path.StartsWith(path)).ToList();
            if (images.Any())
            {
                foreach (var image in images)
                {
                    Context.Images.Remove(entity: image);
                }
                Context.SaveChanges();
            }
        }

        /// <summary>
        ///     Does this file already exist in the database
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns>True if the file is already in the database</returns>
        private bool IsFileInDb(string fullName)
        {
            var file = Context.Images.FirstOrDefault(a => a.FullName == fullName);
            return file != null;
        }

        /// <summary>
        ///     Create a hash string from the bitmap image inside the image file.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private ImageData GetHashString(string fullName)
        {
            ImageData returnData = new ImageData();
            try
            {
                using (Bitmap bmp = (Bitmap)System.Drawing.Image.FromFile(fullName))
                {
                    var bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, bmp.PixelFormat);
                    int totalBytes = bmpData.Stride * bmp.Height;
                    byte[] values = new byte[totalBytes];
                    System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, values, 0, totalBytes);
                    bmp.UnlockBits(bmpData);
                    SHA256 sha = new SHA256Managed();
                    byte[] hash = sha.ComputeHash(values);
                    returnData.hash = BitConverter.ToString(hash).Replace("-", "");
                }

            }
            catch (Exception)
            {
                returnData.hash = null;
                returnData.unreadable = true;
            }

            return returnData;
        }
    }
}