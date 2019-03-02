using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Sitecore.Foundation.Dictionary;
namespace Sitecore.Features.RecentVisit
{
    public class RecentActivitySavedHandler
    {   
        /// <summary>
        /// Gets executed on item saved event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnItemSaved(object sender, EventArgs args)
        {
            // Extract the item from the event Arguments
            Item savedItem = Sitecore.Events.Event.ExtractParameter(args, 0) as Item;

            // Allow only non null items and allow only items from the master database
            if (savedItem != null && savedItem.Database.Name.ToLower() == "master")
            {
                try
                {
                    // Do some kind of template validation to limit only the items you actually want and also restrict save event on scheduler item
                    if (savedItem.TemplateID != ID.Parse("{00000000-0000-0000-0000-000000000000}") && savedItem.TemplateID != ID.Parse(Dictionary.Schedule_TemplateID))
                    {
                        //Read App Data Folder
                        var dataFolder = Sitecore.Configuration.Settings.DataFolder;

                        var fileName = Dictionary.RecentActivityFileName;
                        var tempFileName = Dictionary.TempRecentActivityFileName;
                        var recentActivityFolder = Dictionary.RecentActivityFolder;
                        //Get Files from Server
                        Directory.CreateDirectory(HttpContext.Current.Server.MapPath("~" + dataFolder + "/" + recentActivityFolder));
                        var filePath = HttpContext.Current.Server.MapPath("~" + dataFolder + "/" + recentActivityFolder + "/" + fileName);
                        var tempfilePath = HttpContext.Current.Server.MapPath("~" + dataFolder + "/" + recentActivityFolder + "/" + tempFileName);

                        checkIfRecentActivityFileExist(filePath);
                        //Logic to appending newly generated content to the temp xml file and then appending content from existing xml file to the temp xml file 
                        using (XmlTextReader read = new XmlTextReader(filePath))
                        {
                            using (XmlTextWriter write = new XmlTextWriter(tempfilePath, Encoding.UTF8))
                            {
                                //Create Parent Node with name 'items' in Temp Activity File
                                write.WriteStartElement("items");

                                while (read.Read())
                                {
                                    //Check if current element is parent element from xml file
                                    if (read.NodeType == XmlNodeType.Element && read.Name.ToLower() == "items")
                                    {
                                        // Create new node in temp xml file
                                        writeChildItemtoTempXMLFile(write, savedItem);
                                    }
                                    //Check if current element is child element from xml file
                                    else if (read.NodeType == XmlNodeType.Element && read.Name.ToLower() == "item")
                                    {
                                        // Write Old nodes from xml file to temp xml file
                                        writeChildItemFromXMLtoTempFile(write, read);
                                    }
                                }
                                write.WriteEndElement();
                            }
                        }
                        //Delete backup xml file and move temp file to xml file
                        File.Delete(filePath);
                        File.Move(tempfilePath, filePath);

                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error on check If RecentActivityFileExist ", ex, this);
                }
            }
        }

        /// <summary>
        /// Check if Recent Activity XML file exist
        /// </summary>
        /// <param name="filePath"></param>
        private void checkIfRecentActivityFileExist(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    using (XmlTextWriter writer = new XmlTextWriter(filePath, Encoding.UTF8))
                    {
                        //Create Parent Node with Name 'items'
                        writer.WriteStartElement("items");
                        writer.WriteEndElement();
                        writer.Close();
                    }
                    Console.WriteLine("Recent Activity File created successfully");
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error on check If RecentActivityFileExist ", ex, this);
            }
        }

        /// <summary>
        /// Create new node in temp xml file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="savedItem"></param>
        private void writeChildItemtoTempXMLFile(XmlTextWriter write, Item savedItem)
        {
            //Create child element with name 'item'
            try
            {
                write.WriteStartElement("item");
                write.WriteAttributeString("name", savedItem.Name);
                write.WriteAttributeString("path", savedItem.Paths.FullPath);
                write.WriteAttributeString("id", savedItem.ID.ToString());
                write.WriteAttributeString("language", savedItem.Language.ToString());
                write.WriteAttributeString("version", savedItem.Version.ToString());
                write.WriteAttributeString("icon", savedItem.Appearance.Icon);
                write.WriteEndElement();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error on Write to temp XML File", ex, this);
            }
        }

        /// <summary>
        /// Write Old nodes from xml file to temp xml file
        /// </summary>
        /// <param name="write"></param>
        /// <param name="read"></param>
        private void writeChildItemFromXMLtoTempFile(XmlTextWriter write, XmlTextReader read)
        {
            //Create child element with name 'item'
            try
            {
                write.WriteStartElement("item");
                write.WriteAttributeString("name", read.GetAttribute("name"));
                write.WriteAttributeString("path", read.GetAttribute("path"));
                write.WriteAttributeString("id", read.GetAttribute("id"));
                write.WriteAttributeString("language", read.GetAttribute("language"));
                write.WriteAttributeString("version", read.GetAttribute("version"));
                write.WriteAttributeString("icon", read.GetAttribute("icon"));
                write.WriteEndElement();
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error on Write to from XML file to temp XML File", ex, this);
            }
        }
    }
}