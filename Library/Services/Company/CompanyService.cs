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
    public class CompanyService : ICompanyService
    {
        private readonly IRepository<Company> _repCompany;
        public CompanyService(IRepository<Company> repCompany)
        {
            _repCompany = repCompany;
        }


        public int AddNewCompany(vCompanyAdd model, string userId)
        {
            try
            {
                var item = new Company
                {
                    UserId = userId,
                    Name = model.Name,
                    Address = model.Address,
                    Phone = model.Phone,
                    LinkMan = model.LinkMan,
                    Email = model.Email,
                    Code = GenerateNewCode()
                };

                return _repCompany.InsertGetId(item);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public bool AddNewInfoList(List<vCompanyAdd> list, string userId)
        {
            try
            {
                var infos = list.Select(l => new Company
                {
                    UserId = userId,
                    Name = l.Name,
                    Address = l.Address,
                    Phone = l.Phone,
                    LinkMan = l.LinkMan,
                    Email = l.Email,
                    //BusinessLicense = l.BusinessLicense,
                    //todo:genarate code
                    //Code = l.Code
                });
                _repCompany.InsertRange(infos);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void AddNewInfoListFromExcelStream(Stream stream, string userId)
        {
            try
            {
                var ep = new ExcelPackage(stream);
                var worksheet = ep.Workbook.Worksheets.FirstOrDefault();

                if (worksheet == null) throw new InvalidDataException("上传的文件不正确");
                var rowNumber = worksheet.Dimension.Rows;
                var Cells = worksheet.Cells;

                if (Cells["A1"].Value.ToString() != "企业名称" || Cells["B1"].Value.ToString() != "企业地址" || Cells["C1"].Value.ToString() != "企业代码" || Cells["D1"].Value.ToString() != "企业邮箱" || Cells["E1"].Value.ToString() != "联系人" || Cells["F1"].Value.ToString() != "联系电话") throw new InvalidDataException("上传的文件不正确");

                var infos = new List<Company>();
                for (var i = 2; i <= rowNumber; i++)
                {

                    var info = new Company
                    {
                        UserId = userId,
                        Name = Cells["A" + i].Value.ToString(),
                        Address = Cells["B" + i].Value.ToString(),
                        Code = Cells["C" + i].Value.ToString(),
                        Email = Cells["D" + i].Value.ToString(),
                        LinkMan = Cells["E" + i].Value.ToString(),
                        Phone = Cells["F" + i].Value.ToString()
                    };
                    infos.Add(info);

                }

                _repCompany.InsertRange(infos);
            }
            catch (DbUpdateException e)
            {
                if (e.HResult == -2146233087)
                    throw new DbUpdateException("您插入的企业代码已经存在，不能重复添加");
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public void DeletetById(int id)
        {
            try
            {
                _repCompany.DeleteById(id);
            }
            catch (Exception e)
            {
                throw e;
            }

        }

        public IPagedList<vCompanyList> GetCompanys(vCompanySearch company, int pageIndex, int pageSize)
        {

            var list = _repCompany.Table.Include(c => c.BusinessLicenses).AsNoTracking().Where(c =>
            c.UserId == company.UserId
            && (string.IsNullOrEmpty(company.Name) || c.Name.Contains(company.Name))
              && (string.IsNullOrEmpty(company.Address) || c.Address.Contains(company.Address))
            ).Select(c => new vCompanyList
            {
                Id = c.Id,
                Name = c.Name,
                LinkMan = c.LinkMan,
                Address = c.Address,
                Phone = c.Phone,
                Email = c.Email,
                Code = c.Code,
                BusinessLicenseFilePath = c.BusinessLicenses.OrderByDescending(b => b.Id).FirstOrDefault().Url

            }).OrderBy(c => c.Id).ToList();
            return new PagedList<vCompanyList>(list, pageIndex, pageSize);
        }
        public vCompanyEdit GetCompanyById(int id)
        {
            try
            {
                return _repCompany.Table.Include(c => c.BusinessLicenses).AsNoTracking().Where(c => c.Id == id).Select(c => new vCompanyEdit
                {
                    Id = c.Id,
                    Name = c.Name,
                    Address = c.Address,
                    Phone = c.Phone,
                    Code = c.Code,
                    BusinessLicenseFilePath = c.BusinessLicenses.OrderByDescending(b => b.Id).FirstOrDefault().Url,
                    Email = c.Email,
                    LinkMan = c.LinkMan,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public SelectList GetCompanySelectlistByUserId(string uId, int selectedId)
        {
            try
            {
                var list = _repCompany.Table.Include(c => c.BusinessLicenses).AsNoTracking().Where(c => c.UserId == uId).Select(c => new { c.Id, c.Name });
                if (!list.Any()) return null;
                if (selectedId == 0) return new SelectList(list, "Id", "Name");
                return new SelectList(list, "Id", "Name", selectedId);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public List<SelectListItem> GetCompanySelectListItemsByUserId(string uId, int selectedId)
        {
            try
            {
                var list = _repCompany.Table.Include(c => c.BusinessLicenses).AsNoTracking().Where(c => c.UserId == uId);
                if (!list.Any()) return null;
                return list.Select(c => new SelectListItem() { Value = c.Id.ToString(), Text = c.Name, Selected = selectedId == c.Id }).ToList();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public bool InitCompanyDll(DropDownList ddl, string uId)
        {
            try
            {
                var items = _repCompany.Table.Where(c => c.UserId == uId).ToList();

                ddl.DataSource = items;
                ddl.DataTextField = "Name";
                ddl.DataValueField = "Id";
                ddl.DataBind();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public Company GetByName(string name, string userId)
        {
            try
            {
                var query = _repCompany.Table;
                query = query.Where(q => q.Name == name);
                if (!string.IsNullOrEmpty(userId))
                {
                    query = query.Where(q => q.UserId == userId);
                }
                return query.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public bool UpdateCompany(vCompanyEdit model)
        {
            try
            {
                var item = _repCompany.GetById(model.Id);
                item.Name = model.Name;
                item.Address = model.Address;
                item.Phone = model.Phone;
                item.LinkMan = model.LinkMan;
                item.Email = model.Email;
                _repCompany.Update(item);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        #region private
        string GenerateNewCode()
        {
            try
            {
                var lastCom = _repCompany.DatabaseContext.Set<Company>().AsNoTracking().OrderByDescending(c => c.Id).FirstOrDefault();
                if (lastCom == null)
                {
                    return "INS0000669";
                }
                else
                {
                    var result = "INS0000000";
                    var reg = new Regex(@"\d+");
                    var numInt = Convert.ToInt32(reg.Match(lastCom.Code).Value) + 1;
                    var len = numInt.ToString().Length;
                    result = result.Substring(0, 10 - len);
                    result = result + numInt;
                    return result;
                }

            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
    }
}
