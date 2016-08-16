using System;
using System.Linq;
using Core.Data;
using Domain;
using Models;
using Core.Pager;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web;
using OfficeOpenXml;
using System.ComponentModel;

namespace Services
{
    public class ClaimService : IClaimService
    {
        private readonly IRepository<Company> _repCompany;
        private readonly IRepository<ClaimManagementDetail> _repClaimManagementDetail;
        private readonly IRepository<ClaimFilesList> _repClaimFilesList;
        private readonly IAppUserService _appUserService;
        public ClaimService(IRepository<ClaimFilesList> repClaimFilesList, IRepository<Company> repCompany, IRepository<ClaimManagementDetail> repClaimManagementDetail, IAppUserService appUserService)
        {
            _repClaimFilesList = repClaimFilesList;
            _repCompany = repCompany;
            _repClaimManagementDetail = repClaimManagementDetail;
            _appUserService = appUserService;
        }
        public IPagedList<vClaimManagementDetailList> GetClaimsDetailList(int pageIndex, int pageSize, vClaimManagementDetailListSearch model)
        {
            try
            {
                var user = _appUserService.GetCurrentUser();
                var role = _appUserService.GetRoleByUserId(user.Id);
                if (role == "Admin")
                {
                    if (model.ClaimAccdtDateBegin == DateTime.MinValue)
                    {
                        model.ClaimAccdtDateBegin = DateTime.Now.AddMonths(-1);
                        model.ClaimAccdtDateEnd = DateTime.Now;
                    }

                    var list = _repClaimManagementDetail.Entities.AsNoTracking()
                        .Where(c =>
                    (string.IsNullOrEmpty(model.InsuranceGroupName) || c.InsuranceGroupName.Contains(model.InsuranceGroupName))
                    && (string.IsNullOrEmpty(model.InsuranceName) || c.InsuranceName.Contains(model.InsuranceName))
                    && (string.IsNullOrEmpty(model.InsuranceNo) || c.InsuranceNo.Contains(model.InsuranceNo))
                    && c.ClaimAccdtDate >= model.ClaimAccdtDateBegin
                     && c.ClaimAccdtDate <= model.ClaimAccdtDateEnd
                    )
                    .Select(c => new vClaimManagementDetailList
                    {
                        Id = c.Id,
                        ClaimBatch = c.ClaimBatch,
                        InsuranceGroupName = c.InsuranceGroupName,
                        InsuranceNo = c.InsuranceNo,
                        InsuranceName = c.InsuranceName,
                        ClaimAccdtDate = c.ClaimAccdtDate,
                        ClaimAmt = c.ClaimAmt,
                        ClaimPayDate = c.ClaimPayDate,

                        ExpTotal = c.ExpTotal

                    }).OrderBy(c => c.Id).ToList();
                    return new PagedList<vClaimManagementDetailList>(list, pageIndex, pageSize);
                }
                return new PagedList<vClaimManagementDetailList>(new List<vClaimManagementDetailList>(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public IPagedList<ClaimFilesList> GetClaimFileList(ClaimFilesListSearchModel model, int pageIndex, int pageSize)
        {
            try
            {
                var list = _repClaimFilesList.TableNoTracking;

                if (!model.Author.Equals("Admin", StringComparison.CurrentCultureIgnoreCase))
                {
                    list = list.Where(c => c.Author == model.Author);
                }
                if (model.CreateDate.HasValue)
                {
                    var bDate = model.CreateDate.Value.AddDays(1);
                    var eDate = bDate.AddDays(-3);
                    list = list.Where(c => c.CreateTime <= bDate && c.CreateTime >= eDate);
                }
                if (!string.IsNullOrEmpty(model.FileName))
                {
                    list = list.Where(c => c.ClaimFilesName.StartsWith(model.FileName));
                }
                if (!string.IsNullOrEmpty(model.BatchCode))
                {
                    list = list.Where(c => c.ClaimFilesBatchCode.StartsWith(model.BatchCode));
                }
                list = list.OrderByDescending(c => c.ClaimFilesListID);
                return new PagedList<ClaimFilesList>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public vCompanyEdit GetClaimsDetailById(int id)
        {
            try
            {
                return _repCompany.Table.Include(c => c.BusinessLicenses).AsNoTracking().Where(c => c.Id == id).Select(c => new vCompanyEdit
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Code = c.Code,
                    BusinessLicenseFilePath = c.BusinessLicenses.OrderByDescending(b => b.Id).FirstOrDefault().Url,
                    Email = c.Email,
                    LinkMan = c.LinkMan,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int InsertClaimFileList(HttpPostedFileBase file, string author)
        {
            try
            {

                var ep = new ExcelPackage(file.InputStream);
                var worksheet = ep.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new WarningException("上传的文件内容不能为空");

                var rowNumber = worksheet.Dimension.Rows;
                if (rowNumber <= 1)
                    throw new WarningException("上传的文件内容不能为空");
                //读取excel数据
                var Cells = worksheet.Cells;
                if (Cells["A1"].Value.ToString() != "批次号" || Cells["B1"].Value.ToString() != "图片文件名")
                    throw new WarningException("上传的文件不正确");

                var list = new List<ClaimFilesList>();//先把资料写入临时List,以便判断是否正确
                for (var i = 2; i <= rowNumber; i++)
                {
                    if (Cells["A" + i].Value == null || Cells["B" + i].Value == null)
                        throw new WarningException($"第{i}行数据不能为空");

                    var item = new ClaimFilesList();
                    item.Author = author;
                    item.ClaimFilesStatus = "1";
                    item.ClaimFilesBatchCode = Cells["A" + i].Value.ToString().Trim();
                    item.ClaimFilesName = Cells["B" + i].Value.ToString().Trim();

                    if (string.IsNullOrEmpty(item.ClaimFilesBatchCode) || string.IsNullOrEmpty(item.ClaimFilesName))
                        throw new WarningException($"第{i}行数据不能为空");
                    list.Add(item);
                }
                var result = _repClaimFilesList.InsertRange(list);
                if (result <= 0)
                {
                    throw new WarningException("上传失败,请检查文件内容,请稍后再试");
                }
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
