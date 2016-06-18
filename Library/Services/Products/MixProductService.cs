using Core.Data;
using Domain.Products;
using Microsoft.Owin.Security;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.Products
{
    public class MixProductService : IMixProductService
    {
        private readonly IRepository<MixProduct> _mixProductRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public MixProductService(IRepository<MixProduct> mixProductRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _mixProductRepository = mixProductRepository;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
        }
        public bool Delete(MixProduct item)
        {
            try
            {
                _mixProductRepository.Delete(item, true, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductService：Delete");
                return false;
            }
        }

        public MixProduct GetById(int id)
        {
            try
            {
                return _mixProductRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductService：GetById");
                return null;
            }
        }

        public bool Insert(MixProduct item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _mixProductRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductService：Insert");
                return false;
            }
        }

        public bool Update(MixProduct item)
        {
            try
            {
                _mixProductRepository.Update(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductService：Update");
                return false;
            }
        }
        public List<MixProduct> GetAll()
        {
            try
            {
                var query = _mixProductRepository.TableFromBuffer(72);
                if (query != null && query.Any())
                {
                    query = query.Where(s => s.IsDeleted == false).Include(s => s.ProductMixItem);
                    if (query.Any())
                    {
                        return query.ToList();
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "MixProductService：GetALl");
            }
            return null;
        }
    }
}
