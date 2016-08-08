using Core.Data;
using Core.Pager;
using Domain.Orders;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.events;

using Microsoft.Owin.Security;
using Models.Infrastructure;
using Models.Order;
using Services.FileHelper;
using Services.Products;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf.draw;
using Image = iTextSharp.text.Image;
using ListItem = iTextSharp.text.ListItem;

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
                    table.SetWidths(new float[] { 4, 6, 3, 7, 13, 9, 9, 9, 4, 6 });

                    //table.SetWidthPercentage(new float[] { 0.4f, 8, 10, 10, 14, 8, 6, 6, 4 }, PageSize.A4.Rotate());
                    //table.SetTotalWidth(100);
                    PdfPCell cell;
                    cell = new PdfPHeaderCell() { Colspan = 10, Phrase = new Phrase("《" + order.CompanyName + "》团体保险人员变更名单", OperationPDF.GetFont(fontSize: 12, style: Font.BOLD)), MinimumHeight = 28, HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                    var headFont = OperationPDF.GetFont();
                    headFont.SetColor(255, 255, 255);
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("序号", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("姓名", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("性别", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("证件类型", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("证件号码", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("生日", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("生效日", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("结束日", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("社保", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("投保类型", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
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
                    document.SetMargins(0, 0, 10, 10);
                    PdfWriter.GetInstance(document, stream);
                    document.Open();

                    PdfPTable table = new PdfPTable(9);
                    table.SetWidths(new float[] { 4, 8, 3, 7, 14, 10, 10, 10, 4 });

                    //table.SetWidthPercentage(new float[] { 0.4f, 8, 10, 10, 14, 8, 6, 6, 4 }, PageSize.A4.Rotate());
                    //table.SetTotalWidth(100);
                    PdfPCell cell;
                    cell = new PdfPHeaderCell() { Colspan = 10, Phrase = new Phrase("《" + order.CompanyName + "》团体保险被保险人投保名单", OperationPDF.GetFont(fontSize: 12, style: Font.BOLD)), MinimumHeight = 28, HorizontalAlignment = Element.ALIGN_CENTER };
                    table.AddCell(cell);
                    var headFont = OperationPDF.GetFont();
                    headFont.SetColor(255, 255, 255);
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("序号", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("姓名", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("性别", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("证件类型", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("证件号码", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("生日", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("生效日", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("结束日", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
                    table.AddCell(new PdfPCell() { BackgroundColor = new BaseColor(128, 138, 135), Phrase = new Phrase("有无社保", headFont), HorizontalAlignment = Element.ALIGN_CENTER });
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
                    Paragraph paragraph;
                    PdfPTable table;
                    PdfPCell cell;
                    var pdfWrite = PdfWriter.GetInstance(document, stream);
                    var eventHd = new PageHeaderHandlerAddLogo();

                    document.SetMargins(30, 30, 40, 20);
                    document.Open();
                    eventHd.AddHead(pdfWrite, document);
                    pdfWrite.PageEvent = eventHd;
                    document.Add(new Paragraph("付款通知书/Debit Note\n", OperationPDF.GetTitleFont()) { Alignment = PdfFormField.Q_CENTER, SpacingAfter = 20 });

                    table = new PdfPTable(2) { WidthPercentage = 100 };
                    table.AddCell(
                        new PdfPCell(new Phrase(string.Format($"尊贵的 Dear valued member：{order.CompanyName}"), font)) { BorderWidth = 0, HorizontalAlignment = PdfFormField.Q_LEFT });
                    table.AddCell(
                      new PdfPCell(new Phrase(string.Format($"日期/Date:{DateTime.Now.ToShortDateString()}"), font)) { BorderWidth = 0, HorizontalAlignment = PdfFormField.Q_RIGHT });

                    document.Add(table);
                    document.Add(new Paragraph("感谢您选择保酷平台福利计划！敬请及时支付采购费用。", font) { IndentationLeft = 20 });
                    document.Add(new Paragraph("Thank you for selecting our benefit plan. Please arrange the payment without delay.", font) { IndentationLeft = 20 });

                    #region table
                    table = new PdfPTable(4);
                    table.WidthPercentage = 100;
                    table.SpacingBefore = 30;
                    table.SetWidths(new int[] { 40, 20, 20, 20 });

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

                    #endregion

                    document.Add(
                     new Paragraph(@"请将款项总额汇入以下账号/ Please remit the total amount to the following bank account	", font));

                    table = new PdfPTable(2) { HorizontalAlignment = Element.ALIGN_CENTER };
                    table.SetWidths(new int[2] { 30, 70 });
                    var font1 = OperationPDF.GetFont(fontSize: 12, style: Font.BOLD);
                    table.AddCell(new PdfPCell(new Phrase("开户公司：", font1)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase("金联安保险经纪(北京)有限公司苏州分公司", font)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase("开户银行：", font1)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase("中国建设银行昆山太湖路支行", font)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase("银行帐号：", font1)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase("32201986488052500161", font)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase("转账备注：", font1)) { BorderWidth = 0 });
                    table.AddCell(new PdfPCell(new Phrase(order.OrderNum, font)) { BorderWidth = 0 });
                    document.Add(table);

                    document.Add(new Paragraph("RMB Bank Account Information：", OperationPDF.GetFont(fontSize: 12, style: Font.BOLD)) { IndentationLeft = 57 });
                    document.Add(new Paragraph($"Company Name: Jin Lian An Insurance Brokerage Co.,Ltd Suzhou Branch\nBank Name: Construction Bank of China Kunshan Taihu Road Branch \nAccount No.: 32201986488052500161  \nRemark: {order.OrderNum}") { IndentationLeft = 57 });
                    var fontUnderline = OperationPDF.GetFont(style: Font.UNDERLINE);
                    if (bid != 0)
                    {
                        document.Add(new Phrase("最后付款日期/ Date of payment due:      ", font1));
                        document.Add(new Phrase(order.StartDate.AddDays(5).ToLongDateString(), fontUnderline));
                    }
                    document.Add(new Paragraph() { SpacingAfter = 20 });

                    #region 印章
                    document.Add(new LineSeparator());
                    document.Add(new Paragraph() { SpacingAfter = 20 });

                    var yinzhangParagraph =
                        new Paragraph("         金联安保险经纪(北京)有限公司苏州分公司                          签章：", font)
                        {
                            SpacingBefore = -70,
                            SpacingAfter = 70
                        };
                    var imgSrc = AppDomain.CurrentDomain.BaseDirectory + @"Archive\Template\jinliananZhang.jpg";
                    var yinZhangImage = Image.GetInstance(imgSrc);
                    yinZhangImage.Alignment = Element.ALIGN_RIGHT;
                    //yinZhangImage.SpacingBefore = 50;
                    yinZhangImage.IndentationRight = 40;
                    document.Add(yinZhangImage);
                    document.Add(yinzhangParagraph);

                    #endregion

                    table = new PdfPTable(1) { SpacingAfter = 20, SpacingBefore = 20, WidthPercentage = 100 };
                    var fontWhite = OperationPDF.GetFont();
                    fontWhite.Color = BaseColor.WHITE;
                    cell.BackgroundColor = BaseColor.BLACK;
                    table.AddCell(new PdfPCell(
                            new Phrase(
                                "请在付款前仔细阅读以下注意事项\nPlease read the following notice carefully before processing the payment",
                                fontWhite))
                    { BackgroundColor = BaseColor.BLACK, HorizontalAlignment = Element.ALIGN_CENTER });
                    document.Add(table);

                    document.Add(new Phrase("注意事项/Notes：", font1));
                    var list = new List(List.ORDERED);
                    list.Add(new ListItem(new Phrase("请在最后付款日期之前付款，以便保险公司及时承担相应的保险责任。\nPlease make sure that the premium be finalized before the date of the payment date, so that the insurance company shall undertake the insurance liability accordingly.", font)));

                    list.Add(new ListItem(new Phrase("如遇汇款时汇款金额与付款通知书不符，请及时通知，以免造成不必要的麻烦。\nTo avoid trouble may happens, please inform us when your the amount not in accordance with Debit Note.", font)));

                    list.Add(new ListItem(new Phrase("3.	请在转账时添加上述转账备注，以便更快地确认您的付款。\nPlease add the above remark in the payment transfer, so that we can confirm the payment as soon as possible.", font)));

                    list.Add(new ListItem(new Phrase("若最后付款人与申请表中填写的不同，将以最后付款人名称开具发票。任何情况下发票不能重新开具。\nIf the name of the final payer is different from the one used in your application, please inform us as soon as possible, otherwise the new name will be used for the issue of invoice.Please be noted that the insurance invoice cannot be reissued in any circumstances.", font)));

                    list.Add(new ListItem(new Phrase("请在付款时填写我司账户全称，以免因付款不成功给您带来不便。\nPlease fill out FULL ACCOUNT NAME provided above when arranging the payment to avoid unsuccessful transfer.", font)));

                    document.Add(list);


                    document.Close();
                    pdfWrite.Close();
                    return paths;
                }
                //GetPaymentNoticePdf(oid, bid);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderEmpService：GetPaymentNoticePdf");
            }
            return null;
        }

        public List<string> GetPolicyPdf(int oid)
        {
            try
            {
                var order = _orderService.GetById(oid);
                List<string> pathsToDelete = new List<string>();//生成的临时pdf最后会删除
                var products = _orderitemService.GetList(oid);
                bool hasSafeguardType13 = false;// 住院医疗保险
                string ratioSafeguardType13 = "";
                bool hasSafeguardType10 = false;//住院门诊医疗保险（含生育）
                string ratioSafeguardType10 = "";
                string secondPdfName = null;
                var font = OperationPDF.GetFont();
                if (order != null)
                {

                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var baseFont = OperationPDF.GetBaseFont();

                    var document = new Document();
                    PdfWriter writer = PdfWriter.GetInstance(document, stream);
                    document.Open();
                    #region  manage
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
                    cell2.Phrase = new Phrase("*保险名称", font);
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
                        if (prod.SafeguardCode.Trim() == "SafeguardType13")
                        {
                            hasSafeguardType13 = true;
                            ratioSafeguardType13 = prod.PayoutRatio;
                        }
                        if (prod.SafeguardCode.Trim() == "SafeguardType10")
                        {
                            hasSafeguardType10 = true;
                            ratioSafeguardType10 = prod.PayoutRatio;
                        }

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
                        cell2.Phrase = new Phrase(p.ProdAbatement, font);//免赔
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
                    pathsToDelete.Add(paths[0]);
                    #endregion
                }
                if (!string.IsNullOrEmpty(secondPdfName))
                {

                    List<string> mergePdf = new List<string>() {
                        "/Archive/Template/投保单1.pdf",
                        secondPdfName,
                        "/Archive/Template/投保单2.pdf"
                    };

                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    pathsToDelete.Add(paths[0]);
                    #region 生成第三张pdf 
                    //File.Copy(_httpContext.Request.MapPath("~" + mergePdf[2]), paths[0], true);


                    var document = new Document();
                    var stream = File.Open(paths[0], FileMode.Create);
                    PdfReader reader3 = new PdfReader(_httpContext.Request.MapPath("~" + mergePdf[2]));

                    PdfStamper stamper = new PdfStamper(reader3, stream);
                    StringBuilder mes = new StringBuilder();


                    var mesContent = mes.ToString();

                    float x = 70;
                    float y = reader3.GetPageSize(1).GetTop(20) - 170;

                    if (hasSafeguardType13 || hasSafeguardType10)
                    {
                        if (hasSafeguardType13)
                        {
                            Phrase mesPhrase = new Phrase($"住院赔付比例为{ratioSafeguardType13}", font);
                            ColumnText.ShowTextAligned(
                        stamper.GetOverContent(1), Element.ALIGN_LEFT,
                        mesPhrase, x, y, 0);
                        }
                        if (hasSafeguardType10)
                        {
                            Phrase mesPhrase = new Phrase($"住院/门诊赔付比例为{ratioSafeguardType10}", font);
                            ColumnText.ShowTextAligned(
                    stamper.GetOverContent(1), Element.ALIGN_LEFT,
                    mesPhrase, x, y - 15, 0);
                             mesPhrase = new Phrase($"生育责任限额5000元，免赔3000元", font);
                            ColumnText.ShowTextAligned(
                    stamper.GetOverContent(1), Element.ALIGN_LEFT,
                    mesPhrase, x, y - 30, 0);
                        }
                        Phrase mesPhrase1 = new Phrase("无其他特别约定", font);
                        ColumnText.ShowTextAligned(
                    stamper.GetOverContent(1), Element.ALIGN_LEFT,
                    mesPhrase1, x, y - 45, 0);
                    }
                    else
                    {
                        Phrase mesPhrase = new Phrase("无特别约定", font);
                        ColumnText.ShowTextAligned(
                    stamper.GetOverContent(1), Element.ALIGN_LEFT,
                    mesPhrase, x, y, 0);
                    }

                    stamper.Close();
                    reader3.Close();
                    #endregion
                    reader3 = new PdfReader(paths[0]);//第3张pdf
                    var reader2 = new PdfReader(_httpContext.Request.MapPath("~" + secondPdfName));//第2张pdf
                    var reader1 = new PdfReader(_httpContext.Request.MapPath("~" + mergePdf[0]));//第1张pdf

                    paths = _fileService.GenerateFilePathBySuffix(".pdf");//最终文件
                    stream = new FileStream(paths[0], FileMode.Create);
                    document = new Document();

                    var copy = new PdfCopy(document, stream);

                    //writer = PdfWriter.GetInstance(document, stream);
                    document.Open();
                    copy.AddDocument(reader1);
                    copy.AddDocument(reader2);
                    copy.AddDocument(reader3);
                    document.Close();
                    reader1.Close();
                    reader2.Close();
                    reader3.Close();
                    Task.Run(() =>
                    {
                        pathsToDelete.ForEach(p => File.Delete(p));
                    });

                    //PdfContentByte cb = writer.DirectContent;
                    //PdfImportedPage newPage;
                    ///**/

                    //for (int j = 0; j < mergePdf.Count; j++)
                    //{
                    //    PdfReader reader = new PdfReader(_httpContext.Request.MapPath("~" + mergePdf[j]));
                    //    var totalPages = reader.NumberOfPages;
                    //    for (int i = 1; i <= totalPages; i++)
                    //    {
                    //        document.NewPage();
                    //        newPage = writer.GetImportedPage(reader, i);
                    //        cb.AddTemplate(newPage, 1f, 0, 0, 1f, 0, 0);
                    //    }
                    //}
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
