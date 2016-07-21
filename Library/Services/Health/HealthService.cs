using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.Entity.Infrastructure;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using Core.Data;
using Domain;
using Models;
using Core.Pager;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core;
using Domain.Orders;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Models.Infrastructure;
using Models.Order;

namespace Services
{
    public class HealthService : IHealthService
    {
        private readonly IRepository<HealthCheckProduct> _repHealthProduct;
        private readonly IRepository<HealthOrderDetail> _repHealthOrderDetail;
        private readonly IRepository<HealthOrderMaster> _repHealthOrderMaster;
        private readonly IRepository<Company> _repCompany;
        private readonly IArchiveService _svArchive;
        private readonly ILoggerService _svLogger;
        private readonly IAuthenticationManager _svAuthentication;
        private readonly IFileService _svFile;
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _roleManager;
        public HealthService(IRepository<Company> repCompany, IAuthenticationManager svAuthentication, IFileService svFile, IRepository<HealthOrderMaster> repHealthOrderMaster, ILoggerService svLogger, IArchiveService svArchive, IRepository<HealthOrderDetail> repHealthOrderDetail, AppRoleManager roleManager, AppUserManager userManager, IRepository<HealthCheckProduct> repHealthProduct)
        {
            _repCompany = repCompany;
            _svAuthentication = svAuthentication;
            _repHealthOrderMaster = repHealthOrderMaster;
            _repHealthProduct = repHealthProduct;
            _userManager = userManager;
            _roleManager = roleManager;
            _repHealthOrderDetail = repHealthOrderDetail;
            _svArchive = svArchive;
            _svLogger = svLogger;
            _svFile = svFile;
        }


        public async Task AddHealthMasterAsync(int productId, string author)
        {
            try
            {

                await Task.Run(() =>
                {
                    var master = new HealthOrderMaster()
                    {
                        Author = author,
                        HealthCheckProductId = productId,
                        Status = 1
                    };

                    _repHealthOrderMaster.Insert(master);
                }
            );

            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Error, userName: author);
            }
        }
        public int AddHealthMaster(int productId, string author)
        {
            try
            {
                var p = _repHealthProduct.GetById(productId);
                var master = new HealthOrderMaster()
                {
                    PublicPrice = p.PublicPrice,
                    SellPrice = GetPrivilegePrice(author, p),
                    CommissionMethod = p.CommissionMethod,
                    CommissionRatio = p.CommissionRatio,
                    Author = author,
                    HealthCheckProductId = productId,
                    Status = 1
                };
                return _repHealthOrderMaster.InsertGetId(master);
            }

            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Error, userName: author);
                throw new WarningException("操作失败");
            }
        }
        public List<VCheckProductList> GetHealthProducts(string uId)
        {
            var role = _roleManager.FindById(_userManager.FindById(uId).Roles.First().RoleId);

            var products = _repHealthProduct.Table.AsNoTracking().ToList();

            var list = from p in products
                       select (new VCheckProductList
                       {
                           Id = p.Id,
                           CompanyName = p.CompanyName,
                           ProductCode = p.ProductCode,
                           ProductName = p.ProductName,
                           ProductTypeName = p.ProductTypeName,
                           ProductMemo = p.ProductMemo,
                           CompanyCode = p.CompanyCode,
                           CheckProductPic = p.CheckProductPic,
                           PublicPrice = p.PublicPrice,
                           PrivilegePrice = GetPrivilegePrice(role, p)
                       });
            return list.ToList();
        }
        public HealthOrderMaster GetHealthMaster(int id, string author)
        {
            try
            {
                return _repHealthOrderMaster.Table.FirstOrDefault(h => h.Id == id && h.Author == author);
            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                throw new WarningException("操作有误");
            }
        }
        public HealthOrderMaster GetHealthMaster(int id)
        {
            try
            {
                return _repHealthOrderMaster.GetById(id);
            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                throw new WarningException("操作有误");
            }
        }
        public VHealthEntryInfo GetHealthEntryInfo(int matserId, string author)
        {
            try
            {
                var master =
                    _repHealthOrderMaster.Table.Include(h => h.Company).AsNoTracking().FirstOrDefault(h => h.Id == matserId && h.Author == author);
                if (master == null) { throw new WarningException("操作失败"); }
                else
                {
                    var company = master.Company;
                    if (company == null) company = new Company();
                    return new VHealthEntryInfo()
                    {
                        MasterId = matserId,
                        Status = master.Status,
                        //CompanyName = company.Name,
                        //Linkman = company.LinkMan,
                        //PhoneNumber = company.Phone,
                        //Address = company.Address,
                        CompanyId = company.Id,
                        EmpInfoFileUrl = master.PersonExcelPath
                    };
                }
            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                throw new WarningException("操作有误");
            }
        }
        public IPagedList<HealthOrderDetail> GetHealthOrderDetails(int pageIndex, int pageSize, int masterId)
        {
            try
            {
                var list = _repHealthOrderDetail.TableNoTracking.Where(h => h.HealthOrderMasterId == masterId);
                if (!list.Any()) throw new Exception();
                return new PagedList<HealthOrderDetail>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                return new PagedList<HealthOrderDetail>(new List<HealthOrderDetail>(), pageIndex, pageSize);
            }
        }
        public VCheckProductDetail GetHealthProductById(int id, string uId)
        {
            try
            {
                var role = _roleManager.FindById(_userManager.FindById(uId).Roles.First().RoleId);
                var p = _repHealthProduct.GetById(id);
                return new VCheckProductDetail()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
                    PrivilegePrice = GetPrivilegePrice(role, p),
                    PublicPrice = p.PublicPrice,
                    CheckProductPic = p.CheckProductPic
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public VHealthConfirmPayment GetConfirmPayment(int masterId)
        {
            try
            {
                var master =
                    _repHealthOrderMaster.Table.Include(h => h.HealthOrderDetails)
                        .AsNoTracking()
                        .First(h => h.Id == masterId);
                var result = new VHealthConfirmPayment()
                {
                    MasterId = masterId,
                    Price = master.SellPrice,
                    Count = master.HealthOrderDetails.Count,
                    Amount = master.SellPrice * master.HealthOrderDetails.Count,
                    PaymentNoticePdf = master.PaymentNoticePdf
                };
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public int UploadEmpExcel(HttpPostedFileBase empinfo, int masterId, string author)
        {
            try
            {
                if (empinfo != null && masterId > 0)
                {
                    //打开excel
                    var ep = new ExcelPackage(empinfo.InputStream);
                    var worksheet = ep.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        throw new Exception("上传的文件内容不能为空");
                    var rowNumber = worksheet.Dimension.Rows;
                    var minInsuranceNumber = 0;

                    //若有旧数据先删除
                    var oldInfo = _repHealthOrderDetail.Table.Where(h => h.HealthOrderMasterId == masterId);
                    if (oldInfo.Any())
                    {
                        _repHealthOrderDetail.Delete(oldInfo);

                    }

                    var fileModel = _svArchive.InsertFileInfo(empinfo, author);

                    //读取excel数据
                    var Cells = worksheet.Cells;
                    if (Cells["A1"].Value.ToString().Trim() != "姓名" || Cells["B1"].Value.ToString().Trim() != "性别(男/女)" || Cells["C1"].Value.ToString().Trim() != "出生日期" || Cells["D1"].Value.ToString().Trim() != "证件号码" || Cells["E1"].Value.ToString().Trim() != "婚姻状况" || Cells["F1"].Value.ToString().Trim() != "移动电话" || Cells["G1"].Value.ToString().Trim() != "邮箱地址" || Cells["H1"].Value.ToString().Trim() != "所在城市" || Cells["I1"].Value.ToString().Trim() != "公司名称" || Cells["J1"].Value.ToString().Trim() != "部门" || Cells["K1"].Value.ToString().Trim() != "职位")
                        throw new WarningException("请检查上传文件和下载模板是否拥有相同的列");
                    var list = new List<HealthOrderDetail>();
                    for (var i = 2; i <= rowNumber; i++)
                    {
                        if (Cells["A" + i].Value == null)
                            throw new WarningException($"第{i}行姓名不能为空");

                        if (Cells["B" + i].Value.ToString() != "男" && Cells["B" + i].Value.ToString() != "女")
                            throw new WarningException($"第{i}行性别只能是男或者女");

                        var item = new HealthOrderDetail();
                        item.HealthOrderMasterId = masterId;//批次号
                        item.Name = Cells["A" + i].Value.ToString().Trim();
                        item.Sex = Cells["B" + i].Value.ToString() == "男";
                        item.Birthday = GetBirthday(Cells["C" + i].Value.ToString());
                        item.IdNumber = Cells["D" + i].Value.ToString().Trim();
                        item.Marriage = Cells["E" + i].Value.ToString().Trim();
                        item.Phone = Cells["F" + i].Value.ToString().Trim();
                        item.Email = Cells["G" + i].Value.ToString().Trim();
                        item.Address = Cells["H" + i].Value.ToString().Trim();
                        item.CompanyName = Cells["I" + i].Value.ToString().Trim();
                        item.DepartMent = Cells["J" + i].Value.ToString().Trim();
                        item.Chair = Cells["K" + i].Value.ToString().Trim();
                        list.Add(item);
                    }
                    var result = _repHealthOrderDetail.InsertRange(list);

                    var master = _repHealthOrderMaster.GetById(masterId);
                    if (!string.IsNullOrEmpty(master.PersonExcelPath))
                    {
                        _svArchive.DeleteFileBuUrl(master.PersonExcelPath);
                    }

                    master.PersonExcelPath = fileModel.Url;
                    _repHealthOrderMaster.Update(master);

                    return result;
                }
                else
                {
                    throw new WarningException("请检查上传文件");
                }
            }
            catch (WarningException we)
            {
                throw we;
            }
            catch (Exception exe)
            {
                _svLogger.insert(exe, LogLevel.Error, exe.Message, author);
                throw new WarningException("上传失败");

            }
        }

        public void UpdateMaster(HealthOrderMaster master)
        {
            _repHealthOrderMaster.Update(master);
        }
        #region private

        decimal GetPrivilegePrice(AppRole role, HealthCheckProduct product)
        {
            switch (role.Name)
            {
                case "BusinessDeveloper":
                    return product.BdPrice;
                case "PartnerChannel":
                    return product.ChannelPrice;
                case "CompanyHR":
                    return product.HrPrice;
                default:
                    return product.OtherPrice;
            }
        }
        decimal GetPrivilegePrice(string userName, HealthCheckProduct product)
        {
            var role = _roleManager.FindById(_userManager.FindByName(userName).Roles.First().RoleId);
            switch (role.Name)
            {
                case "BusinessDeveloper":
                    return product.BdPrice;
                case "PartnerChannel":
                    return product.ChannelPrice;
                case "CompanyHR":
                    return product.HrPrice;
                default:
                    return product.OtherPrice;
            }
        }

        DateTime GetBirthday(string str)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) throw new WarningException("生日不能为空");
                return DateTime.Parse(str);

            }
            catch (Exception)
            {
                throw new WarningException("请检查生日是否正确");
            }
        }
        #endregion
    }
}
