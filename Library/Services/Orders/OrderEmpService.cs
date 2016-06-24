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
        public OrderEmpService(ILoggerService loggerService, IRepository<OrderEmployee> orderEmpRepository, IAuthenticationManager authenticationManager, IFileService fileService)
        {
            _loggerService = loggerService;
            _orderEmpRepository = orderEmpRepository;
            _authenticationManager = authenticationManager;
            _fileService = fileService;
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
        public List<OrderEmployee> GetList(int oid)
        {
            try
            {
                var query = _orderEmpRepository.Table;
                if (oid > 0)
                {
                    return query.Where(c => c.order_Id == oid).ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Permission：GetList");
            }
            return new List<OrderEmployee>();
        }
        public IPagedList<OrderEmployeeModel> GetListOfPager(int pageIndex, int pageSize, int oid)
        {
            try
            {
                var query = GetList(oid);
                if (query.Any())
                {
                    return new PagedList<OrderEmployeeModel>(query.Select(s => new OrderEmployeeModel
                    {
                        Id = s.Id,
                        BankCard = s.BankCard,
                        BankName = s.BankName,
                        BirBirthday = s.BirBirthday,
                        Email = s.Email,
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
                var list = GetList(oid);
                if (list.Count > 0)
                {
                    var paths = _fileService.GenerateFilePathBySuffix(".pdf");
                    var stream = new FileStream(paths[0], FileMode.Create);
                    var font = OperationPDF.GetFont();
                    var document = new Document();
                    PdfWriter.GetInstance(document, stream);
                    document.Open();
                    PdfPTable table = new PdfPTable(9);
                    table.SetWidths(new int[] { 4, 8, 10, 10, 14, 8, 6, 6, 4 });
                    PdfPCell cell;
                    cell = new PdfPHeaderCell() { Colspan = 9, Phrase = new Phrase("test", font), HorizontalAlignment = Element.ALIGN_CENTER };
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
    }
}
