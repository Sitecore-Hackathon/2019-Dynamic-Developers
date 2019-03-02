using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Features.RecentVisit
{
    /// <summary>
    /// Extending from Command Class and overriding execute method
    /// </summary>
    public class ShowMostlyVisitedItems : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context == null)
                return;
            try
            {
                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error on executing Recent Activity Command", ex, this);
            }
        }
        public void Refresh(params object[] parameters)
        {
            
        }
    }
}