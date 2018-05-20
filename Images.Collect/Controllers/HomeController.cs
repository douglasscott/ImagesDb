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
    [RoutePrefix("Home")]
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

        public ActionResult Summary()
        {
            return PartialView("_Summary", new ProcessSummary());
        }

        [HttpGet]
        [Route(template: "DataCollection")]
        public ActionResult DataCollection(bool regenerate)
        {
            int count = 0;
            var summary = new ProcessSummary();
            var time = Stopwatch.StartNew();

            // scan folders to load the database
            using (var context = new BuildDb())
            {
                //var recreate = Request.QueryString["cbRecreate"];

                // create the database if it doesn't exist
                if (!context.Context.Database.Exists())
                {
                    context.Context.Database.Create();
                }
                else
                {
                    if (regenerate == true)
                    {
                        context.PurgeOldRecords(defaultDirectory);
                    }
                }

                count = context.hashDirectory(defaultDirectory);

                // display the database summary
                summary.NewRecords = count;
                summary.TotalRecords = context.Context.Images.Count();
                summary.LastProcessTime = time.Elapsed;

            }
            return PartialView("_Summary", summary);
        }
    }
}