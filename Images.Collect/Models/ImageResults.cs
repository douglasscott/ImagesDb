using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Images.Collect.Models
{
    public class ImageResults
    {
        public int id;
        public string Name;
        public string Path;
        public string Hash;

        public ImageResults()
        {
            Name = string.Empty;
            Path = string.Empty;
            Hash = string.Empty;
        }

        public ImageResults(int Id, string name, string path, string hash)
        {
            id = Id;
            Name = name;
            Path = path;
            Hash = hash;
        }
    }
}