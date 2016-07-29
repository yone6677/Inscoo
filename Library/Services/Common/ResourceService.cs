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
            return ConfigurationManager.AppSettings["FileApi"].Trim();
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
        /// <summary>
        /// 上传人员信息
        /// </summary>
        /// <returns></returns>
        public string GetEmployeeInfoTemp()
        {
            return "/Archive/Template/ShangChuanRenYuanXinXi.xlsx";
        }
        /// <summary>
        /// 投保单
        /// </summary>
        /// <returns></returns>
        public string GetInsurancePolicyTemp()
        {
            return "/Archive/Template/投保单.doc";
        }
        /// <summary>
        /// 人员信息变更模板
        /// </summary>
        /// <returns></returns>
        public string GetEmpInfoBuyMoreTemp()
        {
            return "/Archive/Template/RenYuanXinXiGengGaiMuBan.xlsx";
        }
        /// <summary>
        /// 营业执照
        /// </summary>
        /// <returns></returns>
        public string GetBusinessLicenseTemp()
        {
            return "/Archive/Template/yyzz.jpg";
        }
    }
}
