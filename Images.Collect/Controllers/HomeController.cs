using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using Images.Collect.Models;

namespace Images.Collect.Controllers
{
    public class HomeController : Controller
    {
        // image location for testing
        public string defaultDirectory = WebConfigurationManager.AppSettings.GetValues("DefaultDirectory")[0];

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult DataCollection()
        {
            // scan folders to load the database
            using (var context = new BuildDb())
            {
                var recreate = Request.QueryString["cbRecreate"];

                // create the database if it doesn't exist
                if (!context.Context.Database.Exists())
                {
                    context.Context.Database.Create();
                }
                else
                {
                    if (recreate != null)
                    {
                        context.PurgeOldRecords(defaultDirectory);
                    }
                }

                var count = context.hashDirectory(defaultDirectory);

                // display the database summary
                ViewBag.Message = $"{count} new records added to Image database.";

            }
            return View("Index");
        }
    }
}