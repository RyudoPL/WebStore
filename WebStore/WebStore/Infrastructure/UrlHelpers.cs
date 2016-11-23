using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebStore.Infrastructure
{
    /// <summary>
    /// Class that holds all user-defined helpers
    /// </summary>
    public static class UrlHelpers
    {
        public static string IconsPath(this UrlHelper helper, string iconName)
        {
            var IconsFolder = AppConfig.IconFolderRelative;
            var path = Path.Combine(IconsFolder, iconName);
            var absolutePath = helper.Content(path);
            return absolutePath;
        }
    }
}