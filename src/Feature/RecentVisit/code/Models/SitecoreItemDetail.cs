using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Features.RecentVisit.Models
{
    /// <summary>
    /// Model to store item as object
    /// </summary>
    public class SitecoreItemDetail
    {
        public string ID { get; set; }
        public string DisplayName { get; set; }
        public string Path { get; set; }
        public string Language { get; set; }
        public string Version { get; set; }
        public string Icon { get; set; }
    }
}