using Core.Data;
using Core.Pager;
using Domain.Orders;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Owin.Security;
using Models.Infrastructure;
using Models.Order;
using Services.FileHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
        public OrderEmpService(ILoggerService loggerService, IRepository<OrderEmployee> orderEmpRepository, IAuthenticationManager authenticationManager, IFileService fileService,
            IOrderService orderService, IOrderItemService orderitemService)
        {
            _loggerService = loggerService;
            _orderEmpRepository = orderEmpRepository;
            _authenticationManager = authenticationManager;
            _fileService = fileService;
            _orderService = orderService;
            _orderitemService = orderitemService;
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
                _loggerService.insert(e, LogLevel.Warning, "Permission：DeleteById");
                return false;
            }
        }
        public OrderEmployee GetById(int id)
        {
            try
            {
                return _orderEmpRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
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
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
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
                _loggerService.insert(e, LogLevel.Warning, "Permission：Insert");
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
                    return query.Where(c => c.batch_Id == bid).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：GetListByBid");
            }
            return new List<OrderEmployee>();
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
                _loggerService.insert(e, LogLevel.Warning, "Permission：GetListByOid");
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
                    PdfWriter.GetInstance(document, stream);
                    document.Open();
                    PdfPTable table = new PdfPTable(9);
                    table.SetWidths(new int[] { 4, 8, 10, 10, 14, 8, 6, 6, 4 });
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
                        table.AddCell(new Phrase(item.BirBirthday.ToShortDateString(), font));
                        table.AddCell(new Phrase(item.StartDate.ToShortDateString(), font));
                        table.AddCell(new Phrase(item.EndDate.ToShortTimeString(), font));
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
        public List<string> GetPaymentNoticePdf(int oid)
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
                    table.SpacingBefore = 30;
                    table.SetWidths(new int[] { 25, 20, 20, 20 });
                    PdfPCell cell;
                    cell = new PdfPCell() { Rowspan = 2, Phrase = new Phrase("险种名称", font), HorizontalAlignment = PdfPCell.ALIGN_LEFT };
                    table.AddCell(cell);
                    cell = new PdfPCell() { Colspan = 3, Phrase = new Phrase("保费，保额及分类", font), HorizontalAlignment = PdfPCell.ALIGN_CENTER };
                    table.AddCell(cell);
                    table.AddCell(new Phrase("保险金", font));
                    table.AddCell(new Phrase("保额", font));
                    table.AddCell(new Phrase("给付比例", font));
                    foreach (var p in products)
                    {
                        table.AddCell(new Phrase(p.SafeguardName, font));
                        table.AddCell(new Phrase(p.Price.ToString(), font));
                        table.AddCell(new Phrase(p.CoverageSum, font));
                        table.AddCell(new Phrase(p.PayoutRatio, font));
                    }
                    table.AddCell(new Phrase("每人保费总计", font));
                    table.AddCell(new PdfPCell() { Phrase = new Phrase(order.AnnualExpense + "元/人", font), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    table.AddCell(new Phrase("保障人数", font));
                    table.AddCell(new PdfPCell() { Phrase = new Phrase(order.InsuranceNumber + "人", font), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    table.AddCell(new Phrase("金额合计", font));
                    table.AddCell(new PdfPCell() { Phrase = new Phrase((order.InsuranceNumber * order.Pretium) + "元", new Font(baseFont, 12, 1)), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });

                    table.AddCell(new Phrase("保险期间", font));
                    var yearRange = string.Format("一年（{0}-{1}）", order.StartDate.ToShortDateString(), order.EndDate.ToShortDateString());
                    table.AddCell(new PdfPCell() { Phrase = new Phrase(yearRange, font), Colspan = 3, HorizontalAlignment = PdfPCell.ALIGN_CENTER });
                    table.SpacingAfter = 30;
                    document.Add(table);
                    document.Add(new Paragraph("请与" + order.StartDate.AddDays(5).ToShortDateString() + "之前（这个日期为起保日期之后5个工作日）将约定保险金转入下列账户：\n户    名：金联安保险经纪有限公司\n账    户：\n开户  行：\n汇款备注：公司名 - 保单号（后台自动生成）\n", font) { IndentationLeft = 20, SpacingAfter = 40 });
                    document.Add(new Paragraph("敬祝商祺！", font) { Alignment = PdfFormField.Q_LEFT });
                    document.Add(new Paragraph("保酷平台", font) { Alignment = PdfFormField.Q_RIGHT });
                    document.Close();

                    return paths;
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "OrderService：GetPaymentNoticePdf");
            }
            return null;
        }
    }
}
