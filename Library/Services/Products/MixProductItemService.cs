using System;
using Domain.Products;
using Core.Data;
using Microsoft.Owin.Security;
using Services.Infrastructure;

namespace Services.Products
{
    public class MixProductItemService : IMixProductItemService
    {
        private readonly IRepository<MixProductItem> _mixProdItemRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public MixProductItemService(IRepository<MixProductItem> mixProdItemRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _mixProdItemRepository = mixProdItemRepository;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
        }
        public bool Delete(MixProductItem item)
        {
            try
            {
                _mixProdItemRepository.Delete(item, true, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductItemService：Delete");
                return false;
            }
        }

        public MixProductItem GetById(int id)
        {
            try
            {
                return _mixProdItemRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductItemService：GetById");
                return null;
            }
        }

        public bool Insert(MixProductItem item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _mixProdItemRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductItemService：Insert");
                return false;
            }
        }

        public bool Update(MixProductItem item)
        {
            try
            {
                _mixProdItemRepository.Update(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductItemService：Update");
                return false;
            }
        }
    }
}
