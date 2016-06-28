using Domain;
using System;
using System.Collections.Generic;
using System.Web;

namespace Services
{
    public interface IArchiveService
    {
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
        Archive GetById(int id);
    }
}
