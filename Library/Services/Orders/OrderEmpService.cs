using Core.Data;
using Core.Pager;
using Domain.Orders;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using Models.Order;
using Services.FileHelper;
using Services.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
namespace Services.Orders
{
    public class OrderEmpService : IOrderEmpService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<OrderEmployee> _orderEmpRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IFileService _fileService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemService _orderitemService;
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private readonly IOrderEmpTempService _orderEmpTempService;
        private readonly IOrderBatchService _orderBatchService;
        private readonly IProductService _productService;
        public OrderEmpService(ILoggerService loggerService, IRepository<OrderEmployee> orderEmpRepository, IAuthenticationManager authenticationManager, IFileService fileService, IProductService productService,
            IOrderService orderService, IOrderItemService orderitemService, HttpContextBase httpContext, IWebHelper webHelper, IOrderEmpTempService orderEmpTempService, IOrderBatchService orderBatchService)
        {
            _loggerService = loggerService;
            _orderEmpRepository = orderEmpRepository;
            _authenticationManager = authenticationManager;
            _fileService = fileService;
            _orderService = orderService;
            _orderitemService = orderitemService;
            _httpContext = httpContext;
            _webHelper = webHelper;
            _orderEmpTempService = orderEmpTempService;
            _orderBatchService = orderBatchService;
            _productService = productService;
        }
        public bool DeleteById(int id)
        {
            try
            {
                _orderEmpRepository.DeleteById(id);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：DeleteById");
                return false;
            }
        }
        public List<OrderEmployee> GetByInfo(string idNumber, string name)
        {
            try
            {
                var query = _orderEmpRepository.Table;
                if (!string.IsNullOrEmpty(idNumber) && !string.IsNullOrEmpty(name))
                {
                    return query.Where(q => q.Name == name.Trim() && q.IDNumber.ToUpper().Trim() == idNumber.ToUpper().Trim()).ToList();
                }

            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetByInfo");

            }
            return new List<OrderEmployee>();
        }
        public OrderEmployee GetByInfo(string idNumber, string name, int oid)
        {
            try
            {
                var list = new List<OrderEmployee>();
                var orderBatch = _orderService.GetById(oid).orderBatch;
                if (orderBatch.Any())
                {
                    foreach (var b in orderBatch)
                    {
                        var query = GetListByBid(b.Id);
                        if (query.Any())
                        {
                            foreach (var item in query)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    list = list.Where(e => e.IDNumber == idNumber && e.Name == name).ToList();
                    if (list.Count > 0)
                    {
                        return list.FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetByInfo");

            }
            return null;
        }
        public OrderEmployee GetById(int id)
        {
            try
            {
                return _orderEmpRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：Insert");
                return null;
            }
        }
        public bool Update(OrderEmployee item)
        {
            try
            {
                _orderEmpRepository.Update(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：Insert");
                return false;
            }
        }
        public bool Insert(OrderEmployee item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _orderEmpRepository.Insert(item);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：Insert");
                return false;
            }
        }
        public List<OrderEmployee> GetListByBid(int bid)
        {
            try
            {
                var query = _orderEmpRepository.Table;
                if (bid > 0)
                {
                    return query.Where(c => c.batch_Id == bid && c.IsDeleted == false).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetListByBid");
            }
            return new List<OrderEmployee>();
        }
        public decimal GetOrderTotalAmount(int oId)
        {
            try
            {
                var empList = new List<OrderEmployee>() { };
                var order = _orderService.GetById(oId);
                if (order.orderBatch.Any())
                {
                    foreach (var b in order.orderBatch)
                    {
                        var query = _orderEmpRepository.Table;
                        query = query.Where(q => q.batch_Id == b.Id);
                        if (query.Any())
                        {
                            foreach (var e in query)
                            {
                                empList.Add(e);
                            }
                        }
                    }
                }
                if (empList.Count > 0)
                {
                    return empList.Sum(e => e.Premium);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetTotalAmount");
            }
            return 0;
        }
        public List<OrderEmployee> GetListByOid(int oid)
        {
            try
            {
                var list = new List<OrderEmployee>();
                var orderBatch = _orderService.GetById(oid).orderBatch;
                if (orderBatch.Any())
                {
                    foreach (var b in orderBatch)
                    {
                        var query = GetListByBid(b.Id);
                        if (query.Any())
                        {
                            foreach (var item in query)
                            {
                                list.Add(item);
                            }
                        }
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetListByOid");
            }
            return new List<OrderEmployee>();
        }
        public IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int oid)
        {
            try
            {
                var query = GetListByOid(oid);
                if (query.Count > 0)
                {
                    return new PagedList<OrderEmployeeModel>(query.Select(s => new OrderEmployeeModel
                    {
                        Id = s.Id,
                        BankCard = s.BankCard,
                        BankName = s.BankName,
                        Birthday = s.BirBirthday,
                        Email = s.Email,
                        EndDate = s.EndDate,
                        IDNumber = s.IDNumber,
                        IDType = s.IDType,
                        Name = s.Name,
                        PhoneNumber = s.PhoneNumber,
                        Premium = s.Premium,
                        Sex = s.Sex,
                        StartDate = s.StartDate,
                        HasSocialSecurity = s.HasSocialSecurity
                    }).ToList(), pageIndex, pageSize);
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetListOfPager");
            }
            return new PagedList<OrderEmployeeModel>(new List<OrderEmployeeModel>(), pageIndex, pageSize);
        }
        public List<string> GetPdfOfEmpTemp(int bid)
        {
            try
            {
                var list = _orderEmpTempService.GetListByBid(bid);
                if (list.Count > 0)
                {
                    var batch = _orderBatchService.GetById(bid);
                    var order = _orderService.GetById(batch.order_Id);
                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var font = OperationPDF.GetFont();
                    var document = new Document();
                    document.SetPageSize(PageSize.A4.Rotate());
                    document.SetMargins(10, 10, 10, 10);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    PdfPTable table = new PdfPTable(10);
                    table.SetWidths(new float[] { 4, 6, 3, 5, 14, 10, 9, 9, 4, 6 });

                    //table.SetWidthPercentage(new float[] { 0.4f, 8, 10, 10, 14, 8, 6, 6, 4 }, PageSize.A4.Rotate());
                    //table.SetTotalWidth(100);
                    PdfPCell cell;
                    cell = new PdfPHeaderCell() { Colspan = 10, Phrase = new Phrase(order.CompanyName, font), HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                    table.AddCell(new Phrase("序号", font));
                    table.AddCell(new Phrase("姓名", font));
                    table.AddCell(new Phrase("性别", font));
                    table.AddCell(new Phrase("证件类型", font));
                    table.AddCell(new Phrase("证件号码", font));
                    table.AddCell(new Phrase("生日", font));
                    table.AddCell(new Phrase("生效日", font));
                    table.AddCell(new Phrase("结束日", font));
                    table.AddCell(new Phrase("有无社保", font));
                    table.AddCell(new Phrase("投保类型", font));
                    var index = 1;
                    foreach (var item in list)
                    {
                        table.AddCell(new Phrase(index.ToString(), font));
                        table.AddCell(new Phrase(item.Name, font));
                        table.AddCell(new Phrase(item.Sex, font));
                        table.AddCell(new Phrase(item.IDType, font));
                        table.AddCell(new Phrase(item.IDNumber, font));
                        table.AddCell(new Phrase(item.BirBirthday.ToLongDateString(), font));
                        table.AddCell(new Phrase(item.StartDate.ToLongDateString(), font));
                        table.AddCell(new Phrase(item.EndDate.ToLongDateString(), font));
                        table.AddCell(new Phrase(item.HasSocialSecurity, font));
                        if (item.BuyType == 1)
                        {
                            table.AddCell(new Phrase("加保", font));
                        }
                        else
                        {
                            table.AddCell(new Phrase("减保", font));
                        }
                        index++;
                    }
                    document.Add(table);
                    document.Close();
                    return paths;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderItem：GetPdfOfEmpTemp");
            }
            return null;
        }
        public List<string> GetPdf(int oid)
        {
            try
            {
                var list = GetListByOid(oid);
                if (list.Count > 0)
                {
                    var order = _orderService.GetById(oid);
                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var font = OperationPDF.GetFont();
                    var document = new Document();
                    document.SetPageSize(PageSize.A4.Rotate());
                    document.SetMargins(10, 10, 10, 10);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    PdfPTable table = new PdfPTable(9);
                    table.SetWidths(new float[] { 4, 8, 3, 5, 14, 10, 11, 11, 4 });

                    //table.SetWidthPercentage(new float[] { 0.4f, 8, 10, 10, 14, 8, 6, 6, 4 }, PageSize.A4.Rotate());
                    //table.SetTotalWidth(100);
                    PdfPCell cell;
                    cell = new PdfPHeaderCell() { Colspan = 9, Phrase = new Phrase(order.CompanyName, font), HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                    table.AddCell(new Phrase("序号", font));
                    table.AddCell(new Phrase("姓名", font));
                    table.AddCell(new Phrase("性别", font));
                    table.AddCell(new Phrase("证件类型", font));
                    table.AddCell(new Phrase("证件号码", font));
                    table.AddCell(new Phrase("生日", font));
                    table.AddCell(new Phrase("生效日", font));
                    table.AddCell(new Phrase("结束日", font));
                    table.AddCell(new Phrase("有无社保", font));
                    var index = 1;
                    foreach (var item in list)
                    {
                        table.AddCell(new Phrase(index.ToString(), font));
                        table.AddCell(new Phrase(item.Name, font));
                        table.AddCell(new Phrase(item.Sex, font));
                        table.AddCell(new Phrase(item.IDType, font));
                        table.AddCell(new Phrase(item.IDNumber, font));
                        table.AddCell(new Phrase(item.BirBirthday.ToLongDateString(), font));
                        table.AddCell(new Phrase(item.StartDate.ToLongDateString(), font));
                        table.AddCell(new Phrase(item.EndDate.ToLongDateString(), font));
                        table.AddCell(new Phrase(item.HasSocialSecurity, font));
                        index++;
                    }
                    document.Add(table);
                    document.Close();
                    return paths;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderItem：GetPdf");
            }
            return null;
        }
        public List<string> GetPaymentNoticePdf(int oid, int bid = 0)
        {
            try
            {
                var order = _orderService.GetById(oid);
                var products = _orderitemService.GetList(oid);
                if (order != null)
                {
                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var baseFont = OperationPDF.GetBaseFont();
                    var font = OperationPDF.GetFont();
                    var document = new Document();
                    PdfWriter.GetInstance(document, stream);
                    document.Open();
                    document.Add(new Paragraph("付款通知书\n", OperationPDF.GetTitleFont()) { Alignment = PdfFormField.Q_CENTER });
                    document.Add(new Phrase(string.Format("致"), font));
                    document.Add(new Phrase(order.CompanyName, OperationPDF.GetUnderLineFont()));
                    document.Add(new Phrase(":", font));
                    document.Add(new Paragraph("感谢贵公司在保酷平台上采购员工福利保障，具体采购方案如下：", font) { IndentationLeft = 20 });
                    PdfPTable table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 30;
                    table.SetWidths(new int[] { 40, 20, 20, 20 });
                    PdfPCell cell;
                    cell = new PdfPCell() { Rowspan = 2, Phrase = new Phrase("险种名称", font), HorizontalAlignment = Element.ALIGN_LEFT };
                    table.AddCell(cell);
                    cell = new PdfPCell() { Colspan = 3, Phrase = new Phrase("保费，保额及分类", font), HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                    table.AddCell(new Phrase("保险金", font));
                    table.AddCell(new Phrase("保额", font));
                    table.AddCell(new Phrase("给付比例", font));
                    foreach (var p in products)
                    {
                        //table.AddCell(new Phrase(p.SafeguardName, font));
                        var prod = _productService.GetById(p.pid);
                        if (prod != null)
                        {
                            table.AddCell(new Phrase(prod.ProdInsuredName, font));
                        }
                        else
                        {
                            table.AddCell(new Phrase(p.SafeguardName, font));
                        }
                        table.AddCell(new Phrase(p.Price.ToString(), font));
                        table.AddCell(new Phrase(p.CoverageSum, font));
                        table.AddCell(new Phrase(p.PayoutRatio, font));
                    }

                    decimal totalAmount = 0;
                    int InsuranceNumber = 0;
                    if (bid == 0)
                    {
                        totalAmount = GetOrderTotalAmount(oid);//订单总额 包括已减保
                        InsuranceNumber = order.InsuranceNumber;
                    }
                    else//计算某个批次金额
                    {
                        var empTemp = _orderEmpTempService.GetListByBid(bid);
                        if (empTemp.Count > 0)
                        {
                            totalAmount = empTemp.Sum(e => e.Premium);
                        }
                        InsuranceNumber = empTemp.Count;
                    }

                    table.AddCell(new Phrase("每人保费总计", font));
                    table.AddCell(new PdfPCell() { Phrase = new Phrase(order.AnnualExpense + "元/人", font), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    table.AddCell(new Phrase("保障人数", font));
                    table.AddCell(new PdfPCell() { Phrase = new Phrase(InsuranceNumber + "人", font), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    table.AddCell(new Phrase("金额合计", font));


                    table.AddCell(new PdfPCell() { Phrase = new Phrase(totalAmount + "元", new Font(baseFont, 12, 1)), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    table.AddCell(new Phrase("保险期间", font));
                    var yearRange = string.Format("一年（{0}-{1}）", order.StartDate.ToShortDateString(), order.EndDate.ToShortDateString());
                    table.AddCell(new PdfPCell() { Phrase = new Phrase(yearRange, font), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    table.SpacingAfter = 30;
                    document.Add(table);
                    if (bid == 0)
                    {
                        document.Add(new Paragraph("请与" + order.StartDate.AddDays(5).ToShortDateString() + "之前（这个日期为起保日期之后5个工作日）将约定保险金转入下列账户：\n户    名：金联安保险经纪(北京)有限公司苏州分公司\n账    户：32201986488052500161\n开户  行：中国建设银行昆山太湖路支行\n汇款备注：" + order.OrderNum + "\n", font) { IndentationLeft = 20, SpacingAfter = 40 });
                    }
                    else
                    {
                        document.Add(new Paragraph("请将保险金按约定转入下列账户：\n户    名：金联安保险经纪(北京)有限公司苏州分公司\n账    户：32201986488052500161\n开户  行：中国建设银行昆山太湖路支行\n汇款备注：" + order.OrderNum + "\n", font) { IndentationLeft = 20, SpacingAfter = 40 });
                    }
                    document.Add(new Paragraph("敬祝商祺！", font) { Alignment = PdfFormField.Q_LEFT });
                    //   document.Add(new Paragraph("保酷平台", font) { Alignment = PdfFormField.Q_RIGHT });
                    //var img = _httpContext.Request.MapPath("~" + "/Archive/Template/");
                    //Image gif = Image.GetInstance(img + "gongzhang.jpg");
                    //gif.ScalePercent(15f);
                    //gif.Alignment = Element.ALIGN_RIGHT;
                    //document.Add(gif);

                    document.Close();
                    return paths;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetPaymentNoticePdf");
            }
            return null;
        }

        //public void ss()
        //{
        //    string pdfpath = Server.MapPath("PDFs");
        //    string imagepath = Server.MapPath("Images");
        //    Document doc = new Document();
        //    try {
        //        PdfWriter.GetInstance(doc, new FileStream(pdfpath + "/Images.pdf", FileMode.Create));
        //        doc.Open();
        //        doc.Add(new Paragraph("GIF"));
        //        Image gif = Image.GetInstance(imagepath + "/mikesdotnetting.gif");
        //        doc.Add(gif);
        //    }
        //    catch (DocumentException dex)
        //    {
        //        Response.Write(dex.Message);
        //    }
        //    catch (IOException ioex)
        //    {
        //        Response.Write(ioex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        Response.Write(ex.Message);
        //    }
        //    finally { doc.Close(); }
        //}

        public List<string> GetPolicyPdf(int oid)
        {
            try
            {
                var order = _orderService.GetById(oid);
                var products = _orderitemService.GetList(oid);
                string secondPdfName = null;
                if (order != null)
                {
                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var baseFont = OperationPDF.GetBaseFont();
                    var font = OperationPDF.GetFont();
                    var document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();
                    document.Add(new Phrase("B 投保单位信息", OperationPDF.GetFont("STSONG.ttf", 14, 1)));
                    document.Add(new Phrase(string.Format("（加*处为必填事项，下同）"), font));
                    PdfPTable table = new PdfPTable(6);
                    table.WidthPercentage = 100;
                    table.SetWidths(new int[] { 17, 18, 12, 18, 13, 22 });
                    PdfPCell cell = new PdfPCell();
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell.FixedHeight = 22;
                    cell.Phrase = new Phrase("*投保单位全称", font);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(order.CompanyName, font);
                    cell.Colspan = 3;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*行业类别", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*组织机构代码", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("", font);
                    cell.Colspan = 3;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("企业性质", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*单位地址", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    //公司地址
                    cell.Phrase = new Phrase(order.Address, font);
                    cell.Colspan = 3;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*邮政编码", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*联系部门", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*联系人", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(order.Linkman, font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*联系电话", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(order.PhoneNumber, font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*投保日期", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(order.StartDate.ToLongDateString(), font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*参保人数", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(order.InsuranceNumber.ToString(), font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("*单位人数", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    cell.Phrase = new Phrase("", font);
                    cell.Colspan = 1;
                    table.AddCell(cell);
                    document.Add(table);
                    /**/
                    decimal totalAmount = GetOrderTotalAmount(oid);//订单总额 包括已减保
                    document.Add(new Phrase("C 险种保障信息", OperationPDF.GetFont("STSONG.ttf", 14, 1)));
                    document.Add(new Phrase(string.Format("（可附详细清单）"), font));
                    PdfPTable table2 = new PdfPTable(5);
                    table2.WidthPercentage = 100;
                    table2.SetWidths(new int[] { 36, 16, 16, 16, 16 });
                    PdfPCell cell2 = new PdfPCell();
                    cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell2.FixedHeight = 22;
                    cell2.Phrase = new Phrase("*险种名称", font);
                    cell2.Colspan = 1;
                    cell2.Rowspan = 2;
                    table2.AddCell(cell2);
                    cell2.Rowspan = 1;
                    cell2.Phrase = new Phrase("*每个被保险人保额及分类", font);
                    cell2.Colspan = 4;
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("费用", font);
                    cell2.Colspan = 1;
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("I", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("免赔", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("给付比例", font);
                    table2.AddCell(cell2);
                    foreach (var p in products)
                    {
                        var prod = _productService.GetById(p.pid);
                        if (prod != null)
                        {
                            cell2.Phrase = new Phrase(prod.ProdInsuredName, font);
                        }
                        else
                        {
                            cell2.Phrase = new Phrase(p.SafeguardName, font);
                        }
                        table2.AddCell(cell2);
                        cell2.Phrase = new Phrase(p.Price.ToString(), font);
                        table2.AddCell(cell2);
                        cell2.Phrase = new Phrase(p.CoverageSum, font);
                        table2.AddCell(cell2);
                        cell2.Phrase = new Phrase("无", font);//免赔
                        table2.AddCell(cell2);
                        cell2.Phrase = new Phrase(p.PayoutRatio, font);
                        table2.AddCell(cell2);
                    }
                    cell2.Phrase = new Phrase("每人保费", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase(order.AnnualExpense.ToString() + " 元", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("人数", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase(order.InsuranceNumber.ToString() + " 人", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("小计", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase(totalAmount.ToString() + " 元", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    cell2.Phrase = new Phrase("", font);
                    cell2.Phrase = new Phrase("", font);
                    table2.AddCell(cell2);
                    table2.AddCell(cell2);
                    document.Add(table2);
                    PdfPTable table3 = new PdfPTable(4);
                    table3.WidthPercentage = 100;
                    table3.SetWidths(new int[] { 16, 22, 14, 48 });
                    PdfPCell cell3 = new PdfPCell();
                    cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell3.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cell3.FixedHeight = 22;
                    cell3.Phrase = new Phrase("*保额等级划分", font);
                    cell3.Colspan = 1;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("□统一标准 □按职务级别 □按工种 □按工资 □其它方式（详细说明）", font);
                    cell3.Colspan = 3;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("*保险费合计", font);
                    cell3.Colspan = 1;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("（大写）" + _webHelper.CmycurD(totalAmount) + "（小写）" + totalAmount.ToString() + " 元", font);
                    cell3.Colspan = 3;
                    cell3.HorizontalAlignment = Element.ALIGN_LEFT;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("*保险金额合计", font);
                    cell3.Colspan = 1;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("（大写）                                            （小写）", font);
                    cell3.Colspan = 3;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("*保险期间", font);
                    cell3.Colspan = 1;
                    cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase(string.Format("自 {0} 年 {1} 月 {2} 日零时起至 {3} 年 {4} 月 {5} 日二十四时止", order.StartDate.Year,
                        order.StartDate.Month, order.StartDate.Day, order.EndDate.Year, order.EndDate.Month, order.EndDate.Day), font);
                    cell3.Colspan = 3;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("*争议处理方式", font);
                    cell3.Colspan = 1;
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("□诉讼    □仲裁  ", font);
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("仲裁机构", font);
                    table3.AddCell(cell3);
                    cell3.Phrase = new Phrase("", font);
                    table3.AddCell(cell3);
                    document.Add(table3);
                    document.Close();
                    secondPdfName = paths[1];
                }
                if (!string.IsNullOrEmpty(secondPdfName))
                {
                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var baseFont = OperationPDF.GetBaseFont();
                    var font = OperationPDF.GetFont();
                    var document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();

                    PdfContentByte cb = writer.DirectContent;
                    PdfImportedPage newPage;
                    /**/
                    List<string> mergePdf = new List<string>() {
                        "/Archive/Template/投保单1.pdf",
                        secondPdfName,
                        "/Archive/Template/投保单2.pdf"
                    };
                    for (int j = 0; j < mergePdf.Count; j++)
                    {
                        PdfReader reader = new PdfReader(_httpContext.Request.MapPath("~" + mergePdf[j]));
                        var totalPages = reader.NumberOfPages;
                        for (int i = 1; i <= totalPages; i++)
                        {
                            document.NewPage();
                            newPage = writer.GetImportedPage(reader, i);
                            cb.AddTemplate(newPage, 1f, 0, 0, 1f, 0, 0);
                        }
                    }
                    document.Close();
                    return paths;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetPolicyPdf");
            }
            return null;
        }
    }
}
