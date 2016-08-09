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
                list = list.OrderByDescending(c => c.DetailID);
                return new PagedList<CarInsuranceDetail>(list.ToList(), pageIndex, pageSize);

            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}
