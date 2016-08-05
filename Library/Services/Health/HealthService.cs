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
using System.Globalization;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Core;
using Domain.Orders;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using iTextSharp.text.pdf.draw;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using Models.Infrastructure;
using Models.Order;
using NPOI.SS.Formula.Functions;
using Services.FileHelper;

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
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
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
                    CommissionRatio = GetCommissionRatio(author, p),
                    Author = author,
                    HealthCheckProductId = productId,
                    Status = 1
                };
                return _repHealthOrderMaster.InsertGetId(master);
            }

            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, userName: author);
                throw new WarningException("操作失败");
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
        public HealthOrderMaster GetHealthMaster(int id, string author)
        {
            try
            {
                return _repHealthOrderMaster.Table.FirstOrDefault(h => h.Id == id && h.Author == author);
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
                throw new WarningException("操作有误");
            }
        }
        public HealthOrderMaster GetHealthMaster(int id)
        {
            try
            {
                var result = _repHealthOrderMaster.GetById(id);

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
        public VHealthAuditOrder GetHealthAuditOrder(int matserId)
        {
            try
            {
                var master =
                    _repHealthOrderMaster.Table.Include(h => h.HealthOrderDetails).Include(h => h.Company).AsNoTracking().FirstOrDefault(h => h.Id == matserId);
                if (master == null) { throw new WarningException("操作失败"); }
                else
                {
                    var company = master.Company;
                    return new VHealthAuditOrder()
                    {
                        MasterId = matserId,
                        CompanyName = company.Name,
                        Linkman = company.LinkMan,
                        PhoneNumber = company.Phone,
                        Address = company.Address,
                        Price = master.SellPrice,
                        Count = master.HealthOrderDetails.Count,
                        Amount = master.SellPrice * master.HealthOrderDetails.Count,
                    };
                }
            }
            catch (Exception e)
            {
                _svLogger.InsertAsync(e, LogLevel.Error, _svAuthentication.User.Identity.Name);
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
                IQueryable<HealthOrderMaster> list;
                if (search.IsInscooOperator)//如果是operator，查询所有已填写信息的订单
                {
                    if (search.ListType == 2)// 1客户未完成，2客户已完成，未审核，3已审核,4客户已完成
                    {
                        list =
                            _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 4)
                            .AsNoTracking();
                    }
                    else if (search.ListType == 3)
                    {
                        list =
                            _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 11 || h.Status == 14)
                                .AsNoTracking();
                    }
                    else
                    {
                        throw new WarningException("无效的请求");
                    }
                    if (!string.IsNullOrEmpty(search.UserName))
                    {
                        list = list.Where(h => h.Author.Contains(search.UserName));
                    }
                }
                else if (search.IsFinance)//如果是财务，查询所有已审核的订单
                {
                    if (search.ListType == 6)// 6已确认 5 未确认
                    {
                        list =
                            _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 17)
                            .AsNoTracking();
                    }
                    else if (search.ListType == 5)
                    {
                        list =
                            _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct).Where(h => h.Status == 11)
                                .AsNoTracking();
                    }
                    else
                    {
                        throw new WarningException("无效的请求");
                    }
                    if (!string.IsNullOrEmpty(search.UserName))
                    {
                        list = list.Where(h => h.Author.Contains(search.UserName));
                    }
                }
                else//如果不是operator，查询所有当前用户已填写信息的订单
                {

                    if (search.ListType == 1)
                    {
                        list =
                           _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct)
                               .AsNoTracking()
                               .Where(h => h.Author == uName && h.Status < 4);
                    }
                    else if (search.ListType == 4)
                    {
                        list =
                           _repHealthOrderMaster.Table.Include(h => h.HealthCheckProduct)
                               .AsNoTracking()
                               .Where(h => h.Author == uName && h.Status >= 4);
                    }
                    else
                    {
                        throw new WarningException("无效的请求");
                    }
                }
                if (!string.IsNullOrEmpty(search.ProductName))
                {
                    list = list.Where(h => h.HealthCheckProduct.ProductName.Contains(search.ProductName));
                }

                if (!list.Any()) throw new Exception();
                var totalCount = list.Count();
                var pList = from h in list.OrderBy(h => h.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList()
                            let p = h.HealthCheckProduct
                            select (new VHealthAuditList()
                            {
                                MasterId = h.Id,
                                ProductTypeName = p.ProductTypeName,
                                ProductName = p.ProductName,
                                ProductCode = p.ProductCode,
                                PrivilegePrice = h.SellPrice,
                                StatusDes = GetHealthOrderStatus(h.Status),
                                Status = h.Status,
                                Author = h.Author
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
                list.Add(new SelectListItem() { Value = "1", Text = "未填写" });
                list.Add(new SelectListItem() { Value = "4", Text = "已填写" });
                return new SelectList(list, "Value", "Text", 1);
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
                        throw new WarningException("上传的文件内容不能为空");
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
                _svLogger.InsertAsync(exe, LogLevel.Error, exe.Message, author);
                throw new WarningException("上传失败！");

            }
        }

        public void UpdateMaster(HealthOrderMaster master)
        {
            _repHealthOrderMaster.Update(master);
        }

        public async Task GetPaymentNoticePdfAsync(int masterId)
        {
            try
            {
                await Task.Run(() =>
                {

                });
                GetPaymentNoticePdf(masterId);
            }
            catch (Exception exc)
            {
                _svLogger.insert(exc, LogLevel.Warning, "HealthService：GetPaymentNoticePdf");
            }
        }
        public string GetPaymentNoticePdf(int masterId)
        {
            try
            {
                var master = GetConfirmPayment(masterId);
                var masterUp = GetHealthMaster(masterId);
                if (!masterUp.CompanyId.HasValue) return null;
                var paths = _svFile.GenerateFilePathBySuffix(".pdf");
                masterUp.PaymentNoticePdf = "";
                masterUp.PaymentNoticePdf = @"../.." + paths[1];
                UpdateMaster(masterUp);
                var stream = new FileStream(paths[0], FileMode.Create);
                var baseFont = OperationPDF.GetBaseFont();
                var font = OperationPDF.GetFont();
                var document = new Document();
                Paragraph paragraph;
                PdfPTable table;
                PdfPCell cell;
                var pdfWrite = PdfWriter.GetInstance(document, stream);
                var eventHd = new PageHeaderHandlerAddLogo();

                document.SetMargins(30, 30, 40, 20);
                document.Open();
                eventHd.AddHead(pdfWrite, document);
                pdfWrite.PageEvent = eventHd;
                document.Add(new Paragraph("付款通知书/Debit Note\n", OperationPDF.GetTitleFont())
                {
                    Alignment = PdfFormField.Q_CENTER,
                    SpacingAfter = 20
                });

                table = new PdfPTable(2) { WidthPercentage = 100 };
                table.AddCell(
                    new PdfPCell(new Phrase(string.Format($"尊贵的 Dear valued member：{masterUp.Company.Name}"), font))
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

                table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_CENTER };
                table.SetWidths(new int[2] { 30, 70 });
                var font1 = OperationPDF.GetFont(fontSize: 12, style: Font.BOLD);
                table.AddCell(new PdfPCell(new Phrase("单价：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(master.Price.ToString(CultureInfo.InvariantCulture), font))
                {
                    BorderWidth = 0
                });
                table.AddCell(new PdfPCell(new Phrase("购买份数：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(master.Count.ToString(), font)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase("合计金额：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(master.Amount.ToString(CultureInfo.InvariantCulture), font))
                {
                    BorderWidth = 0
                });
                document.Add(table);


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
                table.AddCell(new PdfPCell(new Phrase("转账备注：", font1)) { BorderWidth = 0 });
                table.AddCell(new PdfPCell(new Phrase(master.BaokuOrderCode, font)) { BorderWidth = 0 });
                document.Add(table);

                document.Add(new Paragraph("RMB Bank Account Information：",
                    OperationPDF.GetFont(fontSize: 12, style: Font.BOLD))
                { IndentationLeft = 56 });
                document.Add(
                    new Paragraph(
                        $"Company Name: 上海皓为商务咨询有限公司\nBank Name: 中国工商银行武进路支行 \nAccount No.: 1001213909200135268  \nRemark: {master.BaokuOrderCode}", font)
                    {
                        IndentationLeft = 56
                    });
                var fontUnderline = OperationPDF.GetFont(style: Font.UNDERLINE);

                document.Add(new Paragraph() { SpacingAfter = 10 });
                #region 印章
                document.Add(new LineSeparator());
                document.Add(new Paragraph() { SpacingAfter = 20 });

                var yinzhangParagraph =
                    new Paragraph("           上海皓为商务咨询有限公司                             签章：", font)
                    {
                        SpacingBefore = -70,
                        SpacingAfter = 70
                    };
                var imgSrc = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\health\haoweiYinZhang.jpg";
                var yinZhangImage = iTextSharp.text.Image.GetInstance(imgSrc);
                yinZhangImage.Alignment = Element.ALIGN_RIGHT;
                yinZhangImage.ScaleAbsoluteWidth(80);
                yinZhangImage.ScaleAbsoluteHeight(80);
                //yinZhangImage.SpacingBefore = 50;
                yinZhangImage.IndentationRight = 40;
                document.Add(yinZhangImage);
                document.Add(yinzhangParagraph);

                #endregion


                table = new PdfPTable(1) { SpacingAfter = 20, SpacingBefore = 20, WidthPercentage = 100 };
                var fontWhite = OperationPDF.GetFont();
                fontWhite.Color = BaseColor.WHITE;
                table.AddCell(new PdfPCell(
                    new Phrase(
                        "请在付款前仔细阅读以下注意事项\nPlease read the following notice carefully before processing the payment",
                        fontWhite))
                { BackgroundColor = BaseColor.BLACK, HorizontalAlignment = Element.ALIGN_CENTER });
                document.Add(table);

                document.Add(new Phrase("注意事项/Notes：", font1));
                var list = new List(List.ORDERED);
                list.Add(
                    new iTextSharp.text.ListItem(
                        new Phrase(
                            "请在最后付款日期之前付款，以便保险公司及时承担相应的保险责任。\nPlease make sure that the premium be finalized before the date of the payment date, so that the insurance company shall undertake the insurance liability accordingly.",
                            font)));

                list.Add(
                    new iTextSharp.text.ListItem(
                        new Phrase(
                            "如遇汇款时汇款金额与付款通知书不符，请及时通知，以免造成不必要的麻烦。\nTo avoid trouble may happens, please inform us when your the amount not in accordance with Debit Note.",
                            font)));

                list.Add(
                    new iTextSharp.text.ListItem(
                        new Phrase(
                            "3.	请在转账时添加上述转账备注，以便更快地确认您的付款。\nPlease add the above remark in the payment transfer, so that we can confirm the payment as soon as possible.",
                            font)));

                list.Add(
                    new iTextSharp.text.ListItem(
                        new Phrase(
                            "若最后付款人与申请表中填写的不同，将以最后付款人名称开具发票。任何情况下发票不能重新开具。\nIf the name of the final payer is different from the one used in your application, please inform us as soon as possible, otherwise the new name will be used for the issue of invoice.Please be noted that the insurance invoice cannot be reissued in any circumstances.",
                            font)));

                list.Add(
                    new iTextSharp.text.ListItem(
                        new Phrase(
                            "请在付款时填写我司账户全称，以免因付款不成功给您带来不便。\nPlease fill out FULL ACCOUNT NAME provided above when arranging the payment to avoid unsuccessful transfer.",
                            font)));

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
