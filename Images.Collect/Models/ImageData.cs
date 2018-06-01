using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Images.Collect.Models
{
    /// <summary>
    ///     To hold information returned by the hashDirectory functions
    /// </summary>
    public class ImageData
    {
        public bool unreadable;
        public string hash;
    }
}