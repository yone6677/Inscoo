using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.Entity.Infrastructure;

using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity;

using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using Core.Data;
using Domain;
using Models;
using Core.Pager;
using System.Data.Entity;
using System.Web.Mvc;

namespace Services
{
    public class CarInsuranceService : ICarInsuranceService
    {
        private readonly IRepository<CarInsuranceDetail> _repCarInsuranceDetail;
        public CarInsuranceService(IRepository<CarInsuranceDetail> repCarInsuranceDetail)
        {
            _repCarInsuranceDetail = repCarInsuranceDetail;
        }


        public IPagedList<CarInsuranceDetail> GetDetails(CarInsuranceDetailSearchModel model, int pageIndex, int pageSize)
        {
            try
            {

                var list = _repCarInsuranceDetail.Entities.Where(c => c.IsDelete == 0);
                if (!string.IsNullOrEmpty(model.InsuredName))
                {
                    list = list.Where(c => c.InsuredName.StartsWith(model.InsuredName));
                }
                if (!string.IsNullOrEmpty(model.InsuredCarNo))
                {
                    list = list.Where(c => c.InsuredCarNo.StartsWith(model.InsuredCarNo));
                }
                if (!string.IsNullOrEmpty(model.InsurancePolicy))
                {
                    list = list.Where(c => c.InsurancePolicy.StartsWith(model.InsurancePolicy));
                }
                list = list.OrderByDescending(c => c.DetailID);
                return new PagedList<CarInsuranceDetail>(list.ToList(), pageIndex, pageSize);

            }
            catch (Exception)
            {
                return null;
            }
        }
        public byte[] DownLoadDetails(CarInsuranceDetailSearchModel model)
        {
            try
            {
                using (ExcelPackage ep = new ExcelPackage())
                {
                    var list = _repCarInsuranceDetail.Entities.Where(c => c.IsDelete == 0);
                    if (!string.IsNullOrEmpty(model.InsuredName))
                    {
                        list = list.Where(c => c.InsuredName.StartsWith(model.InsuredName));
                    }
                    if (!string.IsNullOrEmpty(model.InsuredCarNo))
                    {
                        list = list.Where(c => c.InsuredCarNo.StartsWith(model.InsuredCarNo));
                    }
                    if (!string.IsNullOrEmpty(model.InsurancePolicy))
                    {
                        list = list.Where(c => c.InsurancePolicy.StartsWith(model.InsurancePolicy));
                    }
                    var result = list.OrderByDescending(c => c.DetailID).ToList();

                    ExcelWorkbook wb = ep.Workbook;
                    ExcelWorksheet ws = wb.Worksheets.Add("车险列表");

                    //配置文件属性  
                    wb.Properties.Author = "Inscoo";
                    wb.Properties.Comments = "备注";
                    wb.Properties.Company = "Inscoo";
                    wb.Properties.Title = "车险列表";
                    //写数据  
                    ws.Cells[1, 1].Value = "投保人";
                    ws.Cells[1, 2].Value = "车牌";
                    ws.Cells[1, 3].Value = "车辆识别号";
                    ws.Cells[1, 4].Value = "车主身份号码或组织机构代码";
                    ws.Cells[1, 5].Value = "电话";
                    ws.Cells[1, 6].Value = "座位数";
                    ws.Cells[1, 7].Value = "保费";
                    ws.Cells[1, 8].Value = "保单号吗";
                    ws.Cells[1, 9].Value = "起保日期";
                    ws.Cells[1, 10].Value = "结束日期";
                    int i = 2;
                    foreach (var ci in result)
                    {
                        ws.Cells[i, 1].Value = ci.InsuredName;
                        ws.Cells[i, 2].Value = ci.InsuredCarNo;
                        ws.Cells[i, 3].Value = ci.InsuredCarID;
                        ws.Cells[i, 4].Value = ci.InsuredCompanyID;
                        ws.Cells[i, 5].Value = ci.InsuredTel;
                        ws.Cells[i, 6].Value = ci.InsuredSetNumber;
                        ws.Cells[i, 7].Value = ci.InsuredExpense;
                        ws.Cells[i, 8].Value = ci.InsurancePolicy;
                        if (ci.InsuredBeginDate.HasValue)
                        {
                            ws.Cells[i, 9].Value = ci.InsuredBeginDate.Value.ToShortDateString();
                        }
                        else
                        {
                            ws.Cells[i, 9].Value = "";
                        }
                        if (ci.InsuredEndingDate.HasValue)
                        {
                            ws.Cells[i, 10].Value = ci.InsuredEndingDate.Value.ToShortDateString();
                        }
                        else
                        {
                            ws.Cells[i, 10].Value = "";
                        }
                        i++;
                    }

                    return ep.GetAsByteArray();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
