﻿using System;
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
        public string GetEmployeeInfoTemp()
        {
            return "/Archive/Template/上传人员信息.xlsx";
        }
        public string GetInsurancePolicyTemp()
        {
            return "/Archive/Template/投保单.doc";
        }
        public string GetEmpInfoBuyMoreTemp()
        {
            return "/Archive/Template/人员信息变更模板.xlsx";
        }
        public string GetBusinessLicenseTemp()
        {
            return "/Archive/Template/营业执照模版.jpg";
        }
    }
}
