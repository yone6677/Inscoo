using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Products;
using Core.Data;
using Services.Infrastructure;

namespace Services.Product
{
    public class ProductService : IProductService
    {
        private readonly ILoggerService _loggerService;
        private readonly IRepository<ProductItem> _productRepository;
        public ProductService(ILoggerService loggerService, IRepository<ProductItem> productRepository)
        {
            _loggerService = loggerService;
            _productRepository=productRepository;
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

        public ProductItem GetById(int id)
        {
            try
            {
                return _productRepository.GetById(id, true, 72);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：GetById");
                return null;
            }
        }

        public bool Insert(ProductItem item)
        {
            try
            {
                _productRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "ProductService：Insert");
                return false;
            }
        }

        public bool Update(ProductItem item)
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
