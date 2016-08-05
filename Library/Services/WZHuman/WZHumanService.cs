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
using System.Threading.Tasks;
using Models.Infrastructure;

namespace Services
{
    public class WZHumanService : IWZHumanService
    {
        private readonly IRepository<WZHumanMaster> _repWZHumanMaster;
        private readonly IRepository<Archive> _repArchive;
        private readonly ILoggerService _loggerService;
        public WZHumanService(ILoggerService loggerService, IRepository<Archive> repArchive, IRepository<WZHumanMaster> repWZHumanMaster)
        {
            _loggerService = loggerService;
            _repArchive = repArchive;
            _repWZHumanMaster = repWZHumanMaster;
        }


        public async Task AddNewWZHum2anMaster(WZHumanMaster model)
        {
            try
            {

                await _repWZHumanMaster.InsertAsync(model);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Error, "WZHumanService：AddNewWZHumanMaster");
            }
        }
        public IPagedList<WZListModel> GetWZList(WZSearchModel search, int pageIndex, int pageSize)
        {

            var list = _repWZHumanMaster.TableNoTracking.Where(c =>
            c.Author == search.Author || c.Account == search.Author);
            if (!string.IsNullOrEmpty(search.CompanyName))
                list = list.Where(l => l.CompanyName.Contains(search.CompanyName));

            var pageList = list.Select(c => new WZListModel
            {
                Id = c.Id,
                Account = c.Account,
                CompanyName = c.CompanyName,
            }).OrderByDescending(c => c.Id).ToList();
            return new PagedList<WZListModel>(pageList, pageIndex, pageSize);
        }

        public bool HasPerminsion(int masterId, string uName)
        {
            try
            {
                var wz = _repWZHumanMaster.GetById(masterId);
                if (wz.Account == uName || wz.Author == uName) return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
