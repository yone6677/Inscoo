using System;
using System.Configuration;

namespace Services
{
    public class ResourceService : IResourceService
    {


        public int GetFileLimit()
        {
            return int.Parse(ConfigurationManager.AppSettings["FileLimit"].Trim());
        }
        public string GetFileCatalog()
        {
            return ConfigurationManager.AppSettings["FileCatalog"].Trim();
        }
        public string GetFileSystem()
        {
            return ConfigurationManager.AppSettings["FileSystem"].Trim();
        }

        public string GetLogger()
        {
            return ConfigurationManager.AppSettings["Logger"].Trim();
        }
        public bool LogEnable()
        {
            if (ConfigurationManager.AppSettings["LogEnable"].Trim().ToLower() == "true")
            {
                return true;
            }
            return false;
        }
    }
}
