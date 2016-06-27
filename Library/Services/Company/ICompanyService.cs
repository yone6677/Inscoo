using System.Collections.Generic;
using Domain;
using Models;
using System.Linq;
using System.IO;
using System.Web.UI.WebControls;
using Core.Pager;

namespace Services
{
    public interface ICompanyService
    {
        /// <summary>
        /// 添加新的企业信息，返回id
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="businessLicenseFileId"></param>
        /// <param name="businessLicenseFilePath"></param>
        /// <returns></returns>
        int AddNewCompany(vCompanyAdd model, string userId, int businessLicenseFileId, string businessLicenseFilePath);
        bool AddNewInfoList(List<vCompanyAdd> list, string userId);
        /// <summary>
        /// 通过上传文件新增企业信息
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="userId"></param>
        void AddNewInfoListFromExcelStream(Stream stream, string userId);
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void DeletetById(int id);
        PagedList<vCompanyList> GetCompanys(int pageIndex, int pageSize, vCompanySearch company);
        vCompanyEdit GetCompanyById(int id);
        /// <summary>
        /// 初始化企业名称下拉菜单
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="uId"></param>
        /// <returns></returns>
        bool InitCompanyDll(DropDownList ddl, string uId);

        bool UpdateCompany(vCompanyEdit model);

    }
}