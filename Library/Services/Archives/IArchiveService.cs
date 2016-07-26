﻿using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Core.Pager;
using Models;

namespace Services
{
    public interface IArchiveService
    {
        void DeleteCarInsuranceExcel(string excelId);
        /// <summary>
        /// 删除表记录及文件
        /// </summary>
        /// <param name="fileInfo"></param>
        /// <returns></returns>
        Task DeleteFileInfo(Domain.FileInfo fileInfo);
        /// <summary>
        /// 通过url删除表记录及文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task DeleteFileInfo(string url);

        Task DeleteFileBuUrl(string url);
        int InsertByUrl(List<string> fileInfo, string type, int pid, string memo = null);
        /// <summary>
        /// 保酷上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="type"></param>
        /// <param name="pid"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        int Insert(HttpPostedFileBase file, string type, int pid, string memo = null);
        /// <summary>
        /// 最基本的文件上传
        /// </summary>
        /// <param name="file"></param>
        /// <param name="author"></param>
        /// <param name="memo"></param>
        /// <returns>fileinfo id</returns>
        Domain.FileInfo InsertFileInfo(HttpPostedFileBase file, string author, string memo = "");
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
        int inert(Archive item);
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
