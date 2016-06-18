using Core.Data;
using Core.Pager;
using Domain.Common;
using Microsoft.Owin.Security;
using Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services.Common
{
    public class GenericAttributeService : IGenericAttributeService
    {
        private readonly IRepository<GenericAttribute> _genericAttributeRepository;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ILoggerService _loggerService;
        public GenericAttributeService(IRepository<GenericAttribute> genericAttributeRepository, IAuthenticationManager authenticationManager, ILoggerService loggerService)
        {
            _genericAttributeRepository = genericAttributeRepository;
            _loggerService = loggerService;
            _authenticationManager = authenticationManager;
        }

        public bool Delete(int id, bool disable)
        {
            try
            {
                _genericAttributeRepository.DeleteById(id, true, disable);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "GenericAttributeService：Delete");
                return false;
            }
        }

        public GenericAttribute GetById(int id)
        {
            try
            {
                return _genericAttributeRepository.GetById(id);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "GenericAttributeService：GetById");
                return null;
            }
        }

        public bool Insert(GenericAttribute item)
        {
            try
            {
                item.Author = _authenticationManager.User.Identity.Name;
                _genericAttributeRepository.Insert(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "GenericAttributeService：Insert");
                return false;
            }
        }

        public bool Update(GenericAttribute item)
        {
            try
            {
                _genericAttributeRepository.Update(item, true);
                return true;
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "GenericAttributeService：Update");
                return false;
            }
        }
        public List<SelectListItem> GetSelectList(string keyGroup = null)
        {
            var select = new List<SelectListItem>();
            select.Add(new SelectListItem { Text = "请选择", Value = "", Selected = true });
            var query = _genericAttributeRepository.TableFromBuffer(72);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(keyGroup))
                {
                    query = query.Where(s => s.KeyGroup == keyGroup);
                    if (query.Any())
                    {
                        foreach (var s in query)
                        {
                            var item = new SelectListItem();
                            item.Text = s.Key;
                            item.Value = s.Value;
                            select.Add(item);
                        }
                    }
                }
            }
            return select;
        }
        public IPagedList<GenericAttribute> GetListOfPager(int pageIndex = 1, int pageSize = 15, string keyGroup = null, bool IsDeleted = false)
        {
            try
            {
                var query = _genericAttributeRepository.TableFromBuffer(72);
                if (query != null)
                {
                    query = query.Where(s => s.IsDeleted == IsDeleted);
                    if (!string.IsNullOrEmpty(keyGroup))
                    {
                        query = query.Where(s => s.KeyGroup.Contains(keyGroup.Trim()));
                    }
                    if (query.Any())
                    {
                        return new PagedList<GenericAttribute>(query.OrderByDescending(c => c.Id), pageIndex, pageSize);
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetListOfPager");
            }
            return new PagedList<GenericAttribute>(new List<GenericAttribute>(), pageIndex, pageSize);
        }
    }
}
