using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Web.Mvc;
using Core;
using Microsoft.AspNet.Identity;

namespace Services
{
    public class HealthService : IHealthService
    {
        private readonly IRepository<HealthCheckProduct> _repHealthProduct;
        private readonly AppUserManager _userManager;
        private readonly AppRoleManager _roleManager;
        public HealthService(AppRoleManager roleManager, AppUserManager _userManager, IRepository<HealthCheckProduct> repHealthProduct)
        {
            _repHealthProduct = repHealthProduct;
            _userManager = _userManager;
            _roleManager = roleManager;
        }

        public List<vCheckProductList> GetHealthProducts(string uId)
        {
            var role = _roleManager.FindById(_userManager.FindById(uId).Roles.First().RoleId);

            var products = _repHealthProduct.Table.AsNoTracking().ToList();

            var list = from p in products
                       select (new vCheckProductList
                       {
                           Id = p.Id,
                           CompanyName = p.CompanyName,
                           ProductCode = p.ProductCode,
                           ProductName = p.ProductName,
                           ProductType = p.ProductType,
                           ProductMemo = p.ProductMemo,
                           CompanyCode = p.CompanyCode,
                           CheckProductPic = p.CheckProductPic,
                           PublicPrice = p.PublicPrice,
                           PrivilegePrice = GetPrivilegePrice(role, p)
                       });
            return list.ToList();
        }
        public vCheckProductDetail GetHealthProductById(int id, string uId)
        {
            try
            {
                var role = _roleManager.FindById(_userManager.FindById(uId).Roles.First().RoleId);
                var p = _repHealthProduct.GetById(id);
                return new vCheckProductDetail()
                {
                    Id = p.Id,
                    ProductName = p.ProductName,
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
        #endregion
    }
}
