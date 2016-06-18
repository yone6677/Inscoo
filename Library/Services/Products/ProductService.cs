using System;
using Core.Data;
using Services.Infrastructure;
using Domain.Products;
using Microsoft.Owin.Security;

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
    }
}
