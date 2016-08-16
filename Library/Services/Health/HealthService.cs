using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using OfficeOpenXml;
using Core.Data;
using Domain;
using Models;
using Core.Pager;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using Services.FileHelper;
using Models.Cart;

namespace Services
{
    public class HealthService : IHealthService
    {
        private readonly IRepository<HealthCheckProduct> _repHealthProduct;
        private readonly IRepository<HealthOrderDetail> _repHealthOrderDetail;
        private readonly IRepository<HealthOrderMaster> _repHealthOrderMaster;
        private readonly IRepository<HealthFile> _repHealthFile;
        private readonly IRepository<Company> _repCompany;
        private readonly IArchiveService _svArchive;
        private readonly ILoggerService _svLogger;
        private readonly IAuthenticationManager _svAuthentication;
        private readonly IFileService _svFile;
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _roleManager;
        public HealthService(IRepository<Company> repCompany, IAuthenticationManager svAuthentication, IFileService svFile,
            IRepository<HealthOrderMaster> repHealthOrderMaster, ILoggerService svLogger, IArchiveService svArchive,
            IRepository<HealthOrderDetail> repHealthOrderDetail, AppRoleManager roleManager, AppUserManager userManager,
            IRepository<HealthCheckProduct> repHealthProduct, IRepository<HealthFile> repHealthFile)
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
            _repHealthFile = repHealthFile;
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
                        Status = 1,
                        BaokuOrderCode = "HLTH" + DateTime.Now.Ticks.ToString()
                    };

                    _repHealthOrderMaster.Insert(master);
                }
            );

            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
            }
        }
        public HealthOrderMaster AddHealthMaster(int productId, string author, int count)
        {
            try
            {
                var p = _repHealthProduct.GetById(productId);
                var master = new HealthOrderMaster()
                {
                    DateTicks = DateTime.Now.Ticks.ToString(),
                    PublicPrice = p.PublicPrice,
                    SellPrice = GetPrivilegePrice(author, p),
                    CommissionMethod = p.CommissionMethod,
                    CommissionRatio = GetCommissionRatio(author, p),
                    Author = author,
                    HealthCheckProductId = productId,
                    Status = 1,
                    Count = count,
                    BaokuOrderCode = "HLTH" + DateTime.Now.Ticks.ToString()
                };
                _repHealthOrderMaster.Insert(master);
                return master;
            }

            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
                throw new WarningException("操作失败");
            }
        }
        public List<HealthOrderMaster> GetByTicks(string ticks, string author)
        {
            try
            {
                var query = _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(q => q.DateTicks == ticks);
                if (!string.IsNullOrEmpty(author))
                {
                    query = query.Where(q => q.Author == author);
                }
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
            }
            return new List<HealthOrderMaster>();
        }
        public List<HealthOrderMaster> AddHealthMaster(List<CartBuyModel> model, string author)
        {
            try
            {
                var dateTicks = DateTime.Now.Ticks.ToString();
                var Expire = DateTime.Now.AddDays(30);
                var master = new List<HealthOrderMaster>();
                foreach (var m in model)
                {
                    var p = _repHealthProduct.GetById(m.Id);
                    var item = new HealthOrderMaster()
                    {
                        DateTicks = dateTicks,
                        PublicPrice = p.PublicPrice,
                        SellPrice = GetPrivilegePrice(author, p),
                        CommissionMethod = p.CommissionMethod,
                        CommissionRatio = GetCommissionRatio(author, p),
                        Author = author,
                        HealthCheckProductId = m.Id,
                        Status = 1,
                        Count = m.Count,
                        BaokuOrderCode = "HLTH" + dateTicks,
                        Expire = Expire
                    };
                    _repHealthOrderMaster.Insert(item);
                    master.Add(item);
                }
                return master;
            }

            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
                throw new WarningException("操作失败");
            }
        }

        public bool DeleteMaster(int masterID, string author)
        {
            try
            {
                var p = _repHealthOrderMaster.GetById(masterID);
                if (p.Author == author) _repHealthOrderMaster.DeleteById(masterID);
                return true;
            }

            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
                return false;
            }
        }
        public List<VCheckProductList> GetHealthProducts(string uId)
        {
            try
            {

                var role = _roleManager.FindById(_userManager.FindById(uId).Roles.First().RoleId);

                var products = from p in _repHealthProduct.Table.AsNoTracking().OrderBy(h => h.ProductOrder)
                               group p by new
                               {
                                   p.ProductType,
                                   p.ProductName
                               } into pG
                               select pG.FirstOrDefault();

                var list = from p in products.ToList()
                           select (new VCheckProductList
                           {
                               Id = p.Id,
                               ProductType = p.ProductType,
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
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                return new List<VCheckProductList>();
            }
        }
        public List<VCheckProductList> GetHealthProducts(string uId, string productType, string productName)
        {
            try
            {

                var role = _roleManager.FindById(_userManager.FindById(uId).Roles.First().RoleId);

                var products =
                    _repHealthProduct.Table.AsNoTracking()
                        .Where(p => p.ProductType.Trim() == productType && p.ProductName.Trim() == productName.Trim())
                        .OrderBy(h => h.ProductOrder);

                var list = from p in products.ToList()
                           select (new VCheckProductList
                           {
                               Id = p.Id,
                               ProductType = p.ProductType,
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
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                return new List<VCheckProductList>();
            }
        }
        public HealthOrderMaster GetHealthMaster(int id, string dateTicks = "", string author = "")
        {
            try
            {
                var result = _repHealthOrderMaster.GetById(id);
                if (!string.IsNullOrEmpty(author))
                {
                    if (result.Author != author) return null;
                }
                if (!string.IsNullOrEmpty(dateTicks))
                {
                    if (result.DateTicks != dateTicks) return null;
                }
                return result;
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
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
                        DateTicks = master.DateTicks,
                        EmpInfoFileUrl = master.PersonExcelPath
                    };
                }
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                throw new WarningException("操作有误");
            }
        }
        public VHealthAuditOrder GetHealthAuditOrder(int matserId, string dateTicks)
        {
            try
            {
                var master = _repHealthOrderMaster.GetById(matserId);
                //var master =
                //    _repHealthOrderMaster.Table.Include(h => h.Company).AsNoTracking().FirstOrDefault(h => h.Id == matserId && h.DateTicks == dateTicks);
                if (master == null) { throw new WarningException("操作失败"); }
                else
                {
                    var company = master.Company;
                    var servicePeriod = "";
                    if (master.FinancePayDate.HasValue)
                    {
                        servicePeriod = master.FinancePayDate.Value.ToShortDateString() + "至";
                        if (master.ServicePeriod.HasValue)
                        {
                            servicePeriod += master.ServicePeriod.Value.ToShortDateString();
                        }
                    }
                    var ticksGroup = from g in master.HealthOrderDetails.Where(d => d.IsDeleted == false) group g by new { g.Ticks } into a select new { ticks = a.Key.Ticks };
                    var longTicks = new List<long>();
                    foreach (var t in ticksGroup)
                    {
                        longTicks.Add(t.ticks);
                    }
                    if (company == null) company = new Company();
                    return new VHealthAuditOrder()
                    {
                        MasterId = matserId,
                        CompanyName = company.Name ?? "无",
                        Linkman = company.LinkMan ?? "无",
                        PhoneNumber = company.Phone ?? "无",
                        Address = company.Address ?? "无",
                        Price = master.SellPrice,
                        Count = master.Count,
                        Amount = master.SellPrice * master.Count,
                        DateTicks = master.DateTicks,
                        Author = master.Author,
                        CreateTime = master.CreateTime,
                        prodName = master.HealthCheckProduct.ProductName,
                        Expire = master.Expire,
                        ServicePeriod = servicePeriod,
                        ticksGroup = longTicks
                    };
                }
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                throw new WarningException("操作有误");
            }
        }
        public IPagedList<HealthOrderDetail> GetHealthOrderDetails(int pageIndex, int pageSize, int masterId, long ticks)
        {
            try
            {
                var list = _repHealthOrderDetail.TableNoTracking.Where(h => h.HealthOrderMasterId == masterId && h.Ticks == ticks);
                if (!list.Any()) throw new Exception();
                return new PagedList<HealthOrderDetail>(list.ToList(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                return new PagedList<HealthOrderDetail>(new List<HealthOrderDetail>(), pageIndex, pageSize);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="uName"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public IPagedList<VHealthAuditList> GetHealthAuditList(int pageIndex, int pageSize, string uName, VHealthSearch search)
        {
            try
            {
                var newTab = new List<HealthOrderMaster>();
                var query = _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).AsNoTracking();
                if (search.IsInscooOperator)//如果是operator，查询所有已填写信息的订单
                {
                    if (search.ListType == "2")// 1客户未完成，2客户已完成，未审核，3已审核,4客户已完成
                    {
                        query = query.Where(h => h.Status == 4);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 4)
                        //.AsNoTracking();
                    }
                    else if (search.ListType == "3")
                    {
                        query = query.Where(h => h.Status == 11 || h.Status == 14);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 11 || h.Status == 14)
                        //    .AsNoTracking();
                    }
                    else
                    {
                        throw new WarningException("无效的请求");
                    }
                    if (!string.IsNullOrEmpty(search.UserName))
                    {
                        query = query.Where(h => h.Author.Contains(search.UserName));
                    }
                    if (!string.IsNullOrEmpty(search.ProductName))
                    {
                        query = query.Where(h => h.HealthCheckProduct.ProductName.Contains(search.ProductName));
                    }
                    if (query.Any())//有多个项目的订单合并。
                    {
                        var q = from p in query group p by p.DateTicks into g select new { maxId = g.Max(p => p.Id) };
                        foreach (var s in q)
                        {
                            newTab.Add(query.Where(a => a.Id == s.maxId).FirstOrDefault());
                        }
                    }
                }
                else if (search.IsFinance)//如果是财务，查询所有已审核的订单
                {
                    if (search.ListType == "6")// 6已确认 5 未确认
                    {
                        query = query.Where(h => h.Status == 17);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 17)
                        //.AsNoTracking();
                    }
                    else if (search.ListType == "5")
                    {
                        query = query.Where(h => h.Status == 11);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 11)
                        //    .AsNoTracking();
                    }
                    else
                    {
                        throw new WarningException("无效的请求");
                    }
                    if (!string.IsNullOrEmpty(search.UserName))
                    {
                        query = query.Where(h => h.Author.Contains(search.UserName));
                    }
                    if (!string.IsNullOrEmpty(search.ProductName))
                    {
                        query = query.Where(h => h.HealthCheckProduct.ProductName.Contains(search.ProductName));
                    }
                    if (query.Any())//有多个项目的订单合并。
                    {
                        var q = from p in query group p by p.DateTicks into g select new { maxId = g.Max(p => p.Id) };
                        foreach (var s in q)
                        {
                            newTab.Add(query.Where(a => a.Id == s.maxId).FirstOrDefault());
                        }
                    }
                }
                else//如果不是operator，查询所有当前用户已填写信息的订单
                {

                    if (search.ListType == "1")
                    {
                        query = query.Where(h => h.Author == uName && h.Status < 4);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct)
                        //       .AsNoTracking()
                        //       .Where(h => h.Author == uName && h.Status < 4);
                    }
                    else if (search.ListType == "4")
                    {
                        query = query.Where(h => h.Author == uName && h.Status >= 4);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct)
                        //    .AsNoTracking()
                        //    .Where(h => h.Author == uName && h.Status >= 4);
                    }
                    else if (search.ListType == "1|4" || search.ListType == null)
                    {
                        query = query.Where(h => h.Author == uName);
                        //_repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct)
                        //    .AsNoTracking()
                        //    .Where(h => h.Author == uName);
                    }
                    else
                    {
                        throw new WarningException("无效的请求");
                    }
                    newTab = query.ToList();
                }

                if (!newTab.Any()) throw new Exception();
                var totalCount = newTab.Count();
                var pList = from h in newTab.OrderByDescending(h => h.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
                            let p = h.HealthCheckProduct
                            select (new VHealthAuditList()
                            {
                                MasterId = h.Id,
                                ProductTypeName = p.ProductTypeName + "-" + p.CompanyName,
                                ProductName = p.ProductName,
                                ProductCode = p.ProductCode,
                                PrivilegePrice = h.SellPrice,
                                StatusDes = GetHealthOrderStatus(h.Status),
                                Status = h.Status,
                                Author = h.Author,
                                CreateTime = h.CreateTime,
                                DateTicks = h.DateTicks,
                                Count = h.Count,
                                EmpSum = h.HealthOrderDetails.Count,
                                FinConfirmDate = h.FinanceConfirmDate
                            });

                return new PagedList<VHealthAuditList>(pList, pageIndex, pageSize, totalCount);

            }
            catch (WarningException e)
            {
                return new PagedList<VHealthAuditList>(new List<VHealthAuditList>(), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                return new PagedList<VHealthAuditList>(new List<VHealthAuditList>(), pageIndex, pageSize);
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
                    CompanyName = p.CompanyName,
                    ProductType = p.ProductType,
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
        public VHealthConfirmPayment GetConfirmPayment(int masterId, string dateTicks)
        {
            try
            {
                var master =
                    _repHealthOrderMaster.Table
                        .AsNoTracking()
                        .First(h => h.Id == masterId && h.DateTicks == dateTicks);
                var result = new VHealthConfirmPayment()
                {
                    //MasterId = masterId,
                    //Price = master.SellPrice,
                    //Count = master.Count,
                    Amount = master.SellPrice * master.Count,
                    PaymentNoticePdf = master.PaymentNoticePdf ?? "",
                    BaokuOrderCode = master.BaokuOrderCode ?? ""
                };
                return result;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public SelectList GetListType(string uId)
        {
            var roles = _userManager.GetRoles(uId).First();
            var list = new List<SelectListItem>();
            if (roles == "InscooOperator")
            {
                list.Add(new SelectListItem() { Value = "2", Text = "未审核" });
                list.Add(new SelectListItem() { Value = "3", Text = "已审核" });
                return new SelectList(list, "Value", "Text", 2);
            }
            else if (roles == "InscooFinance")
            {
                list.Add(new SelectListItem() { Value = "5", Text = "未确认" });
                list.Add(new SelectListItem() { Value = "6", Text = "已确认" });
                return new SelectList(list, "Value", "Text", 5);
            }
            else
            {
                list.Add(new SelectListItem() { Value = "1|4", Text = "全部" });
                list.Add(new SelectListItem() { Value = "1", Text = "未填写" });
                list.Add(new SelectListItem() { Value = "4", Text = "已填写" });
                return new SelectList(list, "Value", "Text");
            }
        }
        public string UploadEmpExcel(HttpPostedFileBase empinfo, int masterId, string author)
        {
            try
            {
                if (empinfo != null && masterId > 0)
                {
                    var master = _repHealthOrderMaster.GetById(masterId);
                    if (master.Author != author) throw new WarningException("无效操作");
                    //打开excel
                    var ep = new ExcelPackage(empinfo.InputStream);
                    var worksheet = ep.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null)
                        throw new WarningException("上传的文件内容不能为空");
                    var rowNumber = worksheet.Dimension.Rows;
                    var emp = master.HealthOrderDetails.Where(d => d.IsDeleted == false);
                    var empCount = emp.Any() ? emp.Count() : 0;
                    var empAvai = master.Count - empCount;//未上传的人数
                    if ((rowNumber - 1) > empAvai)
                    {
                        throw new WarningException($"人数超出范围,体检人数为 {master.Count} ，最多可再上传 {empAvai} 人，请检查文件");
                    }
                    //若有旧数据先删除
                    //var oldInfo = _repHealthOrderDetail.Table.Where(h => h.HealthOrderMasterId == masterId);
                    //if (oldInfo.Any())
                    //{
                    //    _repHealthOrderDetail.Delete(oldInfo);

                    //}

                    var fileModel = _svArchive.InsertFileInfo(empinfo, author);

                    //读取excel数据
                    var Cells = worksheet.Cells;
                    if (Cells["A1"].Value.ToString().Trim() != "姓名" || Cells["B1"].Value.ToString().Trim() != "性别(男/女)" || Cells["C1"].Value.ToString().Trim() != "出生日期" || Cells["D1"].Value.ToString().Trim() != "证件号码" || Cells["E1"].Value.ToString().Trim() != "婚姻状况" || Cells["F1"].Value.ToString().Trim() != "移动电话" || Cells["G1"].Value.ToString().Trim() != "邮箱地址" || Cells["H1"].Value.ToString().Trim() != "所在城市" || Cells["I1"].Value.ToString().Trim() != "公司名称" || Cells["J1"].Value.ToString().Trim() != "部门" || Cells["K1"].Value.ToString().Trim() != "职位")
                        throw new WarningException("请检查上传文件和下载模板是否拥有相同的列");
                    var list = new List<HealthOrderDetail>();
                    var dateTicks = DateTime.Now.Ticks;
                    for (var i = 2; i <= rowNumber; i++)
                    {
                        if (Cells["A" + i].Value == null)
                            throw new WarningException($"第{i}行 [姓名] 不能为空");
                        if (Cells["B" + i].Value == null)
                            throw new WarningException($"第{i}行 [性别] 不能为空");
                        if (Cells["C" + i].Value == null)
                            throw new WarningException($"第{i}行 [出生日期] 不能为空");
                        if (Cells["D" + i].Value == null)
                            throw new WarningException($"第{i}行 [证件号码] 不能为空");
                        if (Cells["E" + i].Value == null)
                            throw new WarningException($"第{i}行 [婚姻状况] 不能为空");
                        if (Cells["F" + i].Value == null)
                            throw new WarningException($"第{i}行 [移动电话] 不能为空");
                        if (Cells["H" + i].Value == null)
                            throw new WarningException($"第{i}行 [所在城市] 不能为空");

                        if (Cells["B" + i].Value.ToString().Trim() != "男" && Cells["B" + i].Value.ToString().Trim() != "女")
                            throw new WarningException($"第{i}行 [性别] 只能是男或者女");
                        var item = new HealthOrderDetail();
                        item.HealthOrderMasterId = masterId;//批次号
                        item.Name = Cells["A" + i].Value.ToString().Trim();
                        item.Sex = Cells["B" + i].Value.ToString().Trim() == "男";
                        item.Birthday = GetBirthday(Cells["C" + i].Value.ToString(), i);
                        item.IdNumber = Cells["D" + i].Value.ToString().Trim();
                        item.Marriage = Cells["E" + i].Value.ToString().Trim();
                        item.Phone = Cells["F" + i].Value.ToString().Trim();
                        item.Email = Cells["G" + i].Value == null ? "" : Cells["G" + i].Value.ToString().Trim();
                        item.Address = Cells["H" + i].Value.ToString().Trim();
                        item.CompanyName = Cells["I" + i].Value == null ? "" : Cells["I" + i].Value.ToString().Trim();
                        item.DepartMent = Cells["J" + i].Value == null ? "" : Cells["J" + i].Value.ToString().Trim();
                        item.Chair = Cells["K" + i].Value == null ? "" : Cells["K" + i].Value.ToString().Trim();
                        item.Author = author;
                        item.Ticks = dateTicks;
                        list.Add(item);

                    }
                    var result = _repHealthOrderDetail.InsertRange(list);

                    var empFile = new HealthFile()
                    {
                        FId = fileModel.Id,
                        Author = author,
                        HId = master.Id
                    };
                    _repHealthFile.Insert(empFile);

                    //if (!string.IsNullOrEmpty(master.PersonExcelPath))
                    //{
                    //    _svArchive.DeleteFileBuUrl(master.PersonExcelPath);
                    //}
                    var errorMes = "";
                    //if (master.Count > list.Count)
                    //{
                    //    errorMes = "上传人数小于购买人数,支付价格仍会按照购买人数计算.";
                    //}
                    //if (master.Count < list.Count)
                    //{
                    //    master.Count = list.Count;
                    //    errorMes = "上传人数大于购买人数,已自动将购买人数调整为实际上传人数.";
                    //}
                    //master.PersonExcelPath = fileModel.Url;
                    //_repHealthOrderMaster.Update(master);
                    return errorMes;
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
                _svLogger.InsertAsync(exe, LogLevel.Error, exe.Message, author);
                throw new WarningException("上传失败！");

            }
        }

        public void UpdateMaster(HealthOrderMaster master)
        {
            _repHealthOrderMaster.Update(master);
        }

        public async Task GetPaymentNoticePdfAsync(string dateTicks)
        {
            try
            {
                await Task.Run(() =>
                {

                });
                GetPaymentNoticePdf(dateTicks);
            }
            catch (Exception exc)
            {
                _svLogger.insert(exc, LogLevel.Warning, "HealthService：GetPaymentNoticePdf");
            }
        }
        public string GetPaymentNoticePdf(string dateTicks)
        {
            try
            {
                var query = _repHealthOrderMaster.Table.Where(q => q.DateTicks == dateTicks);
                var userName = _svAuthentication.User.Identity.Name;
                query = query.Where(q => q.Author == userName);
                if (!query.Any())
                    return null;
                //var master = GetConfirmPayment(masterId, dateTicks);
                //var masterUp = GetHealthMaster(masterId, dateTicks: dateTicks);
                if (!query.FirstOrDefault().CompanyId.HasValue) return null;
                var paths = _svFile.GenerateFilePathBySuffix(".pdf");
                var newTab = query.ToList();
                foreach (var q in newTab)
                {
                    var item = _repHealthOrderMaster.GetById(q.Id);
                    item.PaymentNoticePdf = "";
                    item.PaymentNoticePdf = @"../.." + paths[1];
                    _repHealthOrderMaster.Update(item);
                }
                //  _repHealthOrderMaster.Update(query);
                var stream = new FileStream(paths[0], FileMode.Create);
                var baseFont = OperationPDF.GetBaseFont();
                var font = OperationPDF.GetFont();
                var fontBold = OperationPDF.GetFont("msyh.ttf", 10, Font.BOLD);
                var document = new Document();
                PdfPTable table;
                var pdfWrite = PdfWriter.GetInstance(document, stream);
                // var eventHd = new PageHeaderHandlerAddLogo();

                document.SetMargins(30, 30, 40, 20);
                document.Open();
                //eventHd.AddHead(pdfWrite, document);
                //pdfWrite.PageEvent = eventHd;
                document.Add(new Paragraph("付款通知书/Debit Note\n", OperationPDF.GetTitleFont())
                {
                    Alignment = PdfFormField.Q_CENTER,
                    SpacingAfter = 20
                });

                table = new PdfPTable(2) { WidthPercentage = 100 };
                table.AddCell(
                    new PdfPCell(new Phrase(string.Format($"尊贵的 Dear valued member：{query.FirstOrDefault().Company.Name}"), font))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = PdfFormField.Q_LEFT
                    });
                table.AddCell(
                    new PdfPCell(new Phrase(string.Format($"日期/Date:{DateTime.Now.ToShortDateString()}"), font))
                    {
                        BorderWidth = 0,
                        HorizontalAlignment = PdfFormField.Q_RIGHT
                    });

                document.Add(table);
                document.Add(new Paragraph("感谢您选择保酷平台福利计划！敬请及时支付采购费用。", font) { IndentationLeft = 20 });
                document.Add(
                    new Paragraph(
                        "Thank you for selecting our benefit plan. Please arrange the payment without delay.", font)
                    {
                        IndentationLeft = 20
                    });
                var font1 = OperationPDF.GetFont(fontSize: 12, style: Font.BOLD);

                table = new PdfPTable(4) { HorizontalAlignment = Element.ALIGN_CENTER };
                table.SetWidths(new int[4] { 15, 35, 15, 35 });
                table.SpacingBefore = 10;
                table.WidthPercentage = 100;
                table.AddCell(new PdfPCell(new Phrase("订单号: \n Policy No", font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(query.FirstOrDefault().BaokuOrderCode, font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("订单有效期: \n Payment Period", font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(query.FirstOrDefault().CreateTime.ToLongDateString() + "-" + query.FirstOrDefault().Expire.ToLongDateString(), font)) { VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                document.Add(table);
                #region table
                decimal amount = 0;
                table = new PdfPTable(4) { HorizontalAlignment = Element.ALIGN_CENTER };
                table.SetWidths(new int[4] { 30, 23, 22, 25 });
                table.SpacingBefore = 10;
                table.WidthPercentage = 100;
                table.AddCell(new PdfPCell(new Phrase("服务项目 \n Policy Type", font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("单价(人民币) \n Price(RMB)", font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("数量 \n Quantity", font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("合计(人民币) \n Sum(RMB)", font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                foreach (var item in query)
                {
                    var product = item.HealthCheckProduct;
                    table.AddCell(new PdfPCell(new Phrase(product.ProductName, font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase(item.SellPrice.ToString(), font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase(item.Count.ToString(), font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase((item.Count * item.SellPrice).ToString(), font)) { BackgroundColor = new BaseColor(236, 242, 250), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, BorderWidth = 0 });
                    amount += item.Count * item.SellPrice;
                }
                table.AddCell(new PdfPCell(new PdfPCell() { Phrase = new Phrase("总金额(人民币)Total Amount(RMB)", font) }) { BackgroundColor = new BaseColor(146, 205, 220), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                table.AddCell(new PdfPCell(new PdfPCell() { Colspan = 3, Phrase = new Phrase(amount.ToString(), font) }) { BackgroundColor = new BaseColor(146, 205, 220), VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER, MinimumHeight = 30, BorderWidth = 0 });
                document.Add(table);
                #endregion

                document.Add(
                    new Paragraph(@"请将款项总额汇入以下账号/ Please remit the total amount to the following bank account	", font) { SpacingAfter = 20, SpacingBefore = 20 });

                table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_CENTER };
                table.SetWidths(new int[2] { 30, 70 });
                table.AddCell(new PdfPCell(new Phrase("开户公司：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("上海皓为商务咨询有限公司", font)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("开户银行：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("中国工商银行武进路支行", font)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("银行帐号：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("1001213909200135268", font)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase($"转账备注：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(query.FirstOrDefault().BaokuOrderCode, font)) { BorderWidth = 0 });
                document.Add(table);

                document.Add(new Paragraph("RMB Bank Account Information：",
                    OperationPDF.GetFont(fontSize: 12, style: Font.BOLD))
                { IndentationLeft = 56 });
                document.Add(
                    new Paragraph(
                        $"Company Name: 上海皓为商务咨询有限公司\nBank Name: 中国工商银行武进路支行 \nAccount No.: 1001213909200135268  \nRemark: {query.FirstOrDefault().BaokuOrderCode ?? ""}", font)
                    {
                        IndentationLeft = 56
                    });


                #region 印章
                var imgSrc = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\hwyz.jpg";
                var yinZhangImage = Image.GetInstance(imgSrc);
                yinZhangImage.Alignment = Element.ALIGN_RIGHT;
                table = new PdfPTable(1) { SpacingBefore = 5 };
                var cell = new PdfPCell(yinZhangImage) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT };
                table.AddCell(cell);
                document.Add(table);
                //document.Add(new LineSeparator());
                document.Add(new Paragraph() { SpacingAfter = 20 });
                #endregion

                var fontUnderline = OperationPDF.GetFont("msyh.ttf", 10);

                fontUnderline.Color = BaseColor.RED;
                document.Add(new Phrase("最后付款日期/ Date of payment due:      ", fontUnderline));
                fontUnderline.SetStyle(Font.UNDERLINE);
                document.Add(new Phrase(query.FirstOrDefault().Expire.ToLongDateString(), fontUnderline));



                document.NewPage();
                table = new PdfPTable(1) { WidthPercentage = 100, SpacingAfter = 20 };
                var fontWhite = OperationPDF.GetFont("msyh.ttf", 11);
                fontWhite.Color = BaseColor.WHITE;
                fontWhite.SetStyle(Font.BOLD);
                var paragraph = new Paragraph("请在付款前仔细阅读以下注意事项\n", fontWhite)
                {
                    Leading = 20
                };
                table.AddCell(new PdfPCell(paragraph)
                { BackgroundColor = new BaseColor(79, 129, 189), BorderWidth = 0, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });
                paragraph = new Paragraph("Please read the following notice carefully before processing the payment", fontWhite)
                {
                    Leading = 20
                };
                table.AddCell(new PdfPCell(paragraph)
                { BackgroundColor = new BaseColor(79, 129, 189), BorderWidth = 0, MinimumHeight = 25, VerticalAlignment = Element.ALIGN_MIDDLE, HorizontalAlignment = Element.ALIGN_CENTER });
                document.Add(table);

                var list = new List(List.ORDERED);
                paragraph = new Paragraph("请在最后付款日期之前付款，以便保险公司及时承担相应的保险责任。", font)
                {
                    Leading = 22
                };
                paragraph.Add(new Paragraph("Please make sure that the premium be finalized before the date of the payment date, so that the insurance company shall undertake the insurance liability accordingly.", font)
                {
                    Leading = 17
                });
                list.Add(new ListItem(paragraph));


                paragraph = new Paragraph("如遇汇款时汇款金额与付款通知书不符，请及时通知，以免造成不必要的麻烦。", font)
                {
                    Leading = 22
                };
                paragraph.Add(new Paragraph("To avoid trouble may happens, please inform us when your the amount not in accordance with Debit Note.", font)
                {
                    Leading = 17
                });
                list.Add(new ListItem(paragraph));

                paragraph = new Paragraph("请在转账时添加上述转账备注，以便更快地确认您的付款。", font)
                {
                    Leading = 22
                };
                paragraph.Add(new Paragraph("Please add the above remark in the payment transfer, so that we can confirm the payment as soon as possible.", font)
                {
                    Leading = 17
                });
                list.Add(new ListItem(paragraph));
                var phrase = new Phrase("若最后付款人与申请表中填写的不同，将以最后付款人名称开具发票。任何情况下发票不能重新开具。", font);
                phrase.Add(new Phrase("不能重新开具。", fontBold));
                paragraph = new Paragraph(phrase)
                {
                    Leading = 22
                };
                phrase = new Phrase("If the name of the final payer is different from the one used in your application, please inform us as soon as possible, otherwise the new name will be used for the issue of invoice.Please be noted that the insurance invoice", font);
                phrase.Add(new Phrase(" cannot ", fontBold));
                phrase.Add(new Phrase("be reissued in any circumstances.", font));
                paragraph.Add(new Paragraph(phrase)
                {
                    Leading = 17
                });

                list.Add(new ListItem(paragraph));

                phrase = new Phrase("请在付款时填写我司", font);
                phrase.Add(new Phrase("账户全称", fontBold));
                phrase.Add(new Phrase("，以免因付款不成功给您带来不便。", font));
                paragraph = new Paragraph(phrase)
                {
                    Leading = 22
                };
                phrase = new Phrase("Please fill out", font);
                phrase.Add(new Phrase(" FULL ACCOUNT NAME ", fontBold));
                phrase.Add(new Phrase("provided above when arranging the payment to avoid unsuccessful transfer.", font));
                paragraph.Add(new Paragraph(phrase)
                {
                    Leading = 17
                });
                list.Add(new ListItem(paragraph));

                document.Add(list);
                document.Close();

                return @"../.." + paths[1];
                //GetPaymentNoticePdf(masterId);
            }
            catch (Exception exc)
            {
                _svLogger.insert(exc, LogLevel.Warning, "HealthService：GetPaymentNoticePdf");
                return "";
            }
        }
        public bool InsertHealthFile(HealthFile item)
        {
            try
            {
                _repHealthFile.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Warning, "HealthService：InsertHealthFile");
            }
            return false;
        }
        public bool DeleteHealthEmp(int id)
        {
            try
            {
                var emp = _repHealthOrderDetail.GetById(id);
                _repHealthOrderDetail.Delete(emp);
            }
            catch (Exception e)
            {
                _svLogger.insert(e, LogLevel.Warning, "HealthService：DeleteHealthEmp");
            }
            return false;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="product">产品</param>
        /// <returns></returns>
        decimal GetCommissionRatio(string userName, HealthCheckProduct product)
        {
            var role = _roleManager.FindById(_userManager.FindByName(userName).Roles.First().RoleId);
            switch (role.Name)
            {
                case "BusinessDeveloper":
                    return product.CommissionRatioBD;
                case "PartnerChannel":
                    return product.CommissionRatioChannel;
                case "CompanyHR":
                    return product.CommissionRatioHR;
                default:
                    return product.CommissionRatioOther;
            }
        }
        DateTime GetBirthday(string str, int rowNum)
        {
            try
            {
                if (string.IsNullOrEmpty(str)) throw new WarningException($"第{rowNum}行生日不能为空");
                return DateTime.Parse(str);

            }
            catch (WarningException e)
            {
                throw e;
            }
            catch (Exception)
            {
                throw new WarningException($"请检查第{rowNum}行生日是否正确");
            }
        }

        string GetHealthOrderStatus(decimal status)
        {
            switch (status.ToString())
            {
                case "1.00":
                    return "订单已确认";
                case "4.00": return "信息已填写";
                case "11.00": return "审核已通过";
                case "14.00": return "审核未通过";
                case "17.00": return "已确认收款";
                default: return "未知状态";
            }
        }

        #endregion
    }
}
