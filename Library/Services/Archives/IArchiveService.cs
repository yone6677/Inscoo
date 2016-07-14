using Domain;
using System;
using System.Collections.Generic;
using System.Web;
using Core.Pager;
using Models;

namespace Services
{
    public interface IArchiveService
    {
        void DeleteCarInsuranceExcel(string excelId);
        int InsertByUrl(List<string> fileInfo, string type, int pid, string memo = null);
        int Insert(HttpPostedFileBase file, string type, int pid, string memo = null);
        /// <summary>
        /// 上传营业执照
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userName"></param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        int InsertBusinessLicense(HttpPostedFileBase file, string userName, int companyId);
        /// <summary>
        /// 上传车险excel
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        string InsertCarInsuranceExcel(HttpPostedFileBase file, string userId, string userName);
        /// <summary>
        /// 上传电子表单
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userName"></param>
        /// <param name="insuranceId"></param>
        /// <returns></returns>
        string InsertCarInsuranceEinsurance(HttpPostedFileBase file, string userName, int insuranceId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <returns>路径</returns>
        string InsertUserPortrait(HttpPostedFileBase file);
        Archive GetById(int id);

        bool Delete(Archive item, bool disable = false);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="createrId"></param>
        /// <returns></returns>
        IPagedList<vCarInsuranceList> GetCarInsuranceExcel(int pageIndex, int pageSize, string createrId = "-1");

        string UpdateCarInsuranceExcel(HttpPostedFileBase file, string excelId);
    }
}
