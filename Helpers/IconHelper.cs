using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace AphidBT.Helpers
{
    public class IconHelper
    {
        public static string GetIcon(string fileExtension)
        {
            var fileExtensions = WebConfigurationManager.AppSettings["AllowableExtensions"].Split(',');
            var imgExtensions = WebConfigurationManager.AppSettings["AllowableImageExtensions"].Split(',');

            foreach (var extension in fileExtensions.Concat(imgExtensions))
            {
                if(extension == fileExtension)
                {
                    return $"/Images/Icons/{extension.TrimStart('.')}.png";
                }

            }

            return "/Images/Icons/default.png";
        }

        public static string GetIconClass(string fileExtension)
        {
            var iconClass = "fa fa-file";
            var returnStr = $"<i class={iconClass}></i>";

            return returnStr;
        }
    }
}