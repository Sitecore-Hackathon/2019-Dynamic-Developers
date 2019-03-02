using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Xml;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Features.RecentVisit.Models;
using Sitecore.Foundation.Dictionary;
using Sitecore.Globalization;
using Sitecore.Resources;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Applications.ContentManager.Galleries;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;

namespace Sitecore.Features.RecentVisit
{
    /// <summary>
    /// Populate Gallery Item XML CodeBehind
    /// </summary>
    public class PopulateFrequentlyVisitedItemcs : GalleryForm
    {
        protected Scrollbox Links;

        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull(message, "message");
            Invoke(message, true);
        }

        protected override void OnLoad(EventArgs e)
        {
            Assert.ArgumentNotNull(e, "e");
            base.OnLoad(e);
            if (!Context.ClientPage.IsEvent)
            {
                StringBuilder result = new StringBuilder();
                RenderRecentActivity(result);
                Links.Controls.Add(new LiteralControl(result.ToString()));
            }
        }

        //Load Recent Activity in Grid Panel from Recent Activity XML File
        private void RenderRecentActivity(StringBuilder result)
        {
            result.Append("<div style=\"font-weight:bold;padding:2px 8px 4px 8px\">" + Translate.Text("Mostly Visited Items")  + ":</div>");

            var dataFolder = Sitecore.Configuration.Settings.DataFolder;
            var fileName = "RecentActivity.xml";
            var recentActivityFolder = Dictionary.RecentActivityFolder;
            var filePath = HttpContext.Current.Server.MapPath("~" + dataFolder + "/"+ recentActivityFolder + "/" + fileName);
            var recentlyVisitedList = new List<SitecoreItemDetail>();
            //Read Activity XML File
            using (XmlTextReader read = new XmlTextReader(filePath))
            {
                while (read.Read())
                {
                    if (read.NodeType == XmlNodeType.Element && read.Name.ToLower() == "item")
                    {
                        //Populate Sitecore Item Detail Object
                        var item_detail = new SitecoreItemDetail();
                        item_detail.ID= read.GetAttribute("id");
                        item_detail.DisplayName = read.GetAttribute("name");
                        item_detail.Path = read.GetAttribute("path");
                        item_detail.Language = read.GetAttribute("language");
                        item_detail.Version = read.GetAttribute("version");
                        item_detail.Icon = read.GetAttribute("icon");
                        recentlyVisitedList.Add(item_detail);
                    }
                }
            }

            //Generate Distinct Items from XML File
            var distictRecenlyVisitedList=recentlyVisitedList.Distinct(new ItemEqualityComparer());
            //Generate List Items
            foreach (var item in distictRecenlyVisitedList.Take(10))
            {
                result.Append(string.Concat(new object[] { "<a href=\"#\" class=\"scLink\" onclick='javascript:return scForm.invoke(\"item:load(id=", item.ID, ",language=", item.Language, ",version=", item.Version, ")\")'>", Images.GetImage(item.Icon, 0x10, 0x10, "absmiddle", "0px 4px 0px 0px"), item.DisplayName, " - [", item.Path, "]</a><br/>" }));
            }
        }
    }

    public class ItemEqualityComparer : IEqualityComparer<SitecoreItemDetail>
    {
        public bool Equals(SitecoreItemDetail x, SitecoreItemDetail y)
        {
            // Two items are equal if their keys are equal.
            return x.ID == y.ID;
        }

        public int GetHashCode(SitecoreItemDetail obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}