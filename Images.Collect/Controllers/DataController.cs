using Images.Collect.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Images.Collect.Controllers
{
    [RoutePrefix("images")]
    public class DataController : ApiController
    {
        /// <summary>
        ///     Search database for images that appear to be duplicates and create a list here.
        ///     We assume that the database has been created before we call this.
        /// </summary>
        /// <returns>List of images with matching hash values</returns>
        [HttpGet]
        [Route(template: "Duplicates")]
        public JsonResult<List<Image>> DuplicateList()
        {
            var records = new List<Image>();
            var hashList = new List<string>();

            try
            {
                using (var context = new BuildDb())
                {
                    if (!context.Context.Database.Exists())
                    {
                        //"The database does not yet exist.  Create it and rerun this.";
                        return null;
                    }

                    var dups = from image in context.Context.Images
                               group image.Hash by image.Hash into grouped
                               where grouped.Count() > 1
                               select grouped;
                    foreach (var duplicate in dups)
                    {
                        hashList.Add(duplicate.Key);
                    }

                    foreach (var hash in hashList)
                    {
                        var duplicateImages = from i in context.Context.Images
                                              where i.Hash == hash
                                              select i;
                        records.AddRange(duplicateImages);
                    }
                }
            }
            catch (Exception ex)
            {

            }

            var json = Json(records);
            return json;
        }
    }
}
