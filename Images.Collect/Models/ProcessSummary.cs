using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Images.Collect.Models
{
    public class ProcessSummary
    {
        public int NewRecords;
        public int TotalRecords;
        public TimeSpan LastProcessTime;

        public ProcessSummary()
        {
            NewRecords = 0;
            TotalRecords = 0;
            LastProcessTime = new TimeSpan();
        }
    }
}