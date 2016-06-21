using System;
using Core.Data;
using Services.Infrastructure;
using Domain.Products;
using Microsoft.Owin.Security;
using Models.Insurance;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web.Mvc;

namespace Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public ProductService(IRepository<Product> productRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _productRepository = productRepository;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
        }

        public bool Delete(int id, bool disable)
        {
            try
            {
                _productRepository.DeleteById(id, true, disable);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：Delete");
                return false;
            }
        }

        public Product GetById(int id)
        {
            try
            {
                return _productRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：GetById");
                return null;
            }
        }

        public bool Insert(Product item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _productRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：Insert");
                return false;
            }
        }

        public bool Update(Product item)
        {
            try
            {
                _productRepository.Update(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：Update");
                return false;
            }
        }
        public List<Product> GetList(string company = null, string SafeguardCode = null, string CoverageSum = null, string PayoutRatio = null, string InsuredWho = null)
        {
            try
            {
                var query = _productRepository.TableFromBuffer(72);
                if (!string.IsNullOrEmpty(company))
                {
                    query = query.Where(q => q.InsuredCom == company);
                }
                if (!string.IsNullOrEmpty(SafeguardCode))
                {
                    query = query.Where(q => q.SafeguardCode == SafeguardCode);
                }
                if (!string.IsNullOrEmpty(CoverageSum))
                {
                    query = query.Where(q => q.CoverageSum == CoverageSum);
                }
                if (!string.IsNullOrEmpty(PayoutRatio))
                {
                    query = query.Where(q => q.PayoutRatio == PayoutRatio);
                }
                if (!string.IsNullOrEmpty(InsuredWho))
                {
                    query = query.Where(q => q.InsuredWho == InsuredWho);
                }
                if (query.Any())
                {
                    return query.ToList();
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：GetList");
            }
            return new List<Product>();
        }
        public List<ProductListModel> GetProductListForInscoo(string company, string productType, int staffsNum,string InsuredWho)
        {
            try
            {
                var model = new List<ProductListModel>();
                var query = _productRepository.TableFromBuffer(72).OrderBy(q => q.SafeguardName);
                var slectList = _productRepository.TableFromBuffer(72);
                var gQuey = from p in query
                            group p by new
                            {
                                p.SafeguardName,
                                p.ProdType,
                                p.InsuredCom,
                                p.SafeguardCode
                            } into g
                            select new
                            {
                                SafeguardName = g.Key.SafeguardName,
                                ProdType = g.Key.ProdType,
                                InsuredCom = g.Key.InsuredCom,
                                SafeguardCode = g.Key.SafeguardCode
                            };
                if (gQuey.Any())
                {
                    if (!string.IsNullOrEmpty(company))
                    {
                        gQuey = gQuey.Where(s => s.InsuredCom == company);
                    }
                    if (!string.IsNullOrEmpty(productType))
                    {
                        gQuey = gQuey.Where(s => s.ProdType == productType);
                    }
                    foreach (var s in gQuey)
                    {
                        var productItem = new ProductListModel();
                        productItem.SafeguardName = s.SafeguardName;
                        productItem.InsuredCom = s.InsuredCom;
                        productItem.SafeguardCode = s.SafeguardCode;
                        var CoverageSumList = new List<SelectListItem>();
                        var selectCove = slectList.Where(c => c.SafeguardName == s.SafeguardName && c.InsuredCom == s.InsuredCom && c.ProdType == s.ProdType && c.InsuredWho == InsuredWho).OrderBy(c => c.Id);
                        foreach (var a in selectCove)
                        {
                            switch (staffsNum)
                            {
                                case 1:
                                    if (a.HeadCount3 == "-")
                                    {
                                        continue;
                                    }
                                    break;
                                case 2:
                                    if (a.HeadCount5 == "-")
                                    {
                                        continue;
                                    }                                   
                                    break;
                                case 3:
                                    if (a.HeadCount11 == "-")
                                    {
                                        continue;
                                    }
                                    break;
                                case 4:
                                    if (a.HeadCount31 == "-")
                                    {
                                        continue;
                                    }
                                    break;
                                case 5:
                                    if (a.HeadCount51 == "-")
                                    {
                                        continue;
                                    }
                                    break;
                                case 6:
                                    if (a.HeadCount100 == "-")
                                    {
                                        continue;
                                    }
                                    break;
                            }
                            if (!productItem.CoverageSumList.Where(pc => pc.Text == a.CoverageSum).Any())
                            {
                                var coveSumList = new SelectListItem();
                                coveSumList.Text = a.CoverageSum;
                                coveSumList.Value = a.Id.ToString();
                                productItem.CoverageSumList.Add(coveSumList);
                            }
                            if (a.PayoutRatio != "无" && !productItem.PayoutRatioList.Where(pr => pr.Text == a.PayoutRatio).Any())
                            {
                                var PayoutRatioList = new SelectListItem();
                                PayoutRatioList.Text = a.PayoutRatio;
                                PayoutRatioList.Value = a.PayoutRatio;
                                productItem.PayoutRatioList.Add(PayoutRatioList);
                            }
                        }
                        if (productItem.CoverageSumList.Count > 0)
                        {
                            model.Add(productItem);
                        }
                    }
                    if (model.Count > 0)
                    {
                        return model;
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：GetProductListForInscoo");
            }
            return null;
        }
    }
}
