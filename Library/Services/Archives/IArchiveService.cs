﻿using Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using Core.Pager;
using Models;
using Models.Api.Archive;
using Models.Infrastructure;
using System.Linq;

namespace Services
{
    public interface IArchiveService
    {
        void DeleteCarInsuranceExcel(int insuranceId);
        void DeleteMemberInsuranceExcel(int insuranceId);
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
        bool Delete(Archive item, bool disable = false);
        bool DeleteById(int id, bool disable, string author);
        IQueryable<Archive> GetByTypeAndPId(int pId, string type);
        CarInsurance GetCarEInsuranceUrl(int insuranceId, string uKey);
        MemberInsurance GetMemberEInsuranceUrl(int insuranceId, string uKey);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="pId"></param>
        /// <param name="type">文件类型</param>
        /// <returns></returns>
        IPagedList<Archive> GetPagelistByTypeAndPId(int pageIndex, int pageSize, int pId, string type);
        IPagedList<Archive> GetWZFileDataModels(WZFileSearchModel model, int pageIndex, int pageSize);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="createrId"></param>
        /// <returns></returns>
        IPagedList<vCarInsuranceList> GetCarInsuranceExcel(int fileType, int pageIndex, int pageSize, string createrId = "-1");
        IPagedList<vMemberInsuranceList> GetMemberInsuranceExcel(int fileType, int pageIndex, int pageSize, string author = "-1");
        int InsertByWechat(DownLoadWechatFileApi model);
        int InsertByUrl(List<string> fileInfo, string type, int pid, string memo = null);
        /// <summary>
        /// 保存网络资源文件，只保存url其他留空
        /// </summary>
        /// <returns></returns>
        Archive InsertByUrl(string url, string type, int pid, string memo = null);
        int InsertBySaveResult(SaveResultModel model, string type, int pid, string memo = null);
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
        string InsertCarInsuranceExcel(HttpPostedFileBase file, string userId, string userName, int fileType);
        string InsertMemberInsuranceExcel(HttpPostedFileBase file, string userName, string fileTypeName, int fileType);
        /// <summary>
        /// 上传电子表单
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userName"></param>
        /// <param name="insuranceId"></param>
        /// <param name="uKey"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        string InsertCarInsuranceEinsurance(HttpPostedFileBase file, string userName, int insuranceId, string uKey, string code);
        string InsertMemberInsuranceEinsurance(HttpPostedFileBase file, string userName, int insuranceId, string uKey, string code);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="userId"></param>
        /// <returns>路径</returns>
        string InsertUserPortrait(HttpPostedFileBase file);
        Archive GetById(int id);
        int inert(Archive item);
        /// <summary>
        /// 吴中人力资源 
        /// 上传保险人员名单
        /// </summary>
        /// <param name="file"></param>
        /// <param name="author"></param>
        /// <param name="masterId"></param>
        /// <param name="memo"></param>
        /// <returns></returns>
        string InsertWZInsurants(HttpPostedFileBase file, string author, int masterId, string memo = "", string type = "WZHuman");

        string UpdateCarInsuranceExcel(HttpPostedFileBase file, string excelId, string author);
        void UploadCarInsuranceEOrderCode(string code, int insuranceId, string uKey);
        void UploadMemberInsuranceEOrderCode(string code, int insuranceId, string uKey);
    }
}
