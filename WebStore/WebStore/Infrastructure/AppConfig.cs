using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebStore.Infrastructure
{
    public class AppConfig
    {
        private static string _iconFolderRelative = ConfigurationManager.AppSettings["IconsFolder"];
        public static string IconFolderRelative
        {
            get
            {
                return _iconFolderRelative;
            }
        }
    }
}