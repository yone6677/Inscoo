using Core.Data;
using Core.Pager;
using Domain.Common;
using Microsoft.Owin.Security;
using Models.Common;
using Models.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Services
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
        public GenericAttribute GetByKey(string key, string keyGroup, string value)
        {
            try
            {
                var query = _genericAttributeRepository.TableFromBuffer(72);
                if (query != null)
                {
                    if (!string.IsNullOrEmpty(key))
                    {
                        query = query.Where(s => s.Key == key.Trim());
                    }
                    if (!string.IsNullOrEmpty(keyGroup))
                    {
                        query = query.Where(s => s.KeyGroup == keyGroup.Trim());
                    }
                    if (!string.IsNullOrEmpty(value))
                    {
                        query = query.Where(s => s.Value == value.Trim());
                    }
                    if (query.Any())
                    {
                        return query.FirstOrDefault();
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "GenericAttributeService：GetById");
            }
            return null;
        }

        public List<GenericAttribute> GetByGroup(string keyGroup)
        {
            try
            {
                return _genericAttributeRepository.TableFromBuffer(72).Where(a => a.KeyGroup == keyGroup).OrderBy(a => a.Sequence).ToList();
            }
            catch (Exception e)
            {
                return new List<GenericAttribute>();
            }
        }
        public SelectList GetSelectListByGroup(string keyGroup, string selectedValue)
        {
            try
            {
                var list = _genericAttributeRepository.TableFromBuffer(72).Where(a => a.KeyGroup == keyGroup).OrderBy(a => a.Sequence).ToList();
                return new SelectList(list, "Value", "Key", selectedValue);
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public ListBox GetListBoxtByGroup(string keyGroup, string selectedValue)
        {
            try
            {
                var list = _genericAttributeRepository.TableFromBuffer(72).Where(a => a.KeyGroup == keyGroup).ToList();
                var listBox = new ListBox();
                //listBox.

                return new ListBox();
            }
            catch (Exception e)
            {
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
        public List<SelectListItem> GetSelectList(string keyGroup = null, bool isTip = false, bool IsDeleted = false)
        {
            var select = new List<SelectListItem>();
            if (isTip)
            {
                select.Add(new SelectListItem { Text = "请选择", Value = "0", Selected = true });
            }
            var query = _genericAttributeRepository.TableFromBuffer(72);
            if (query != null)
            {
                if (!string.IsNullOrEmpty(keyGroup))
                {
                    query = query.Where(s => s.KeyGroup == keyGroup && s.IsDeleted == IsDeleted);
                    if (query.Any())
                    {
                        query = query.OrderBy(a => a.Sequence);
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
        public List<GenericAttributeModel> GetList(string keyGroup = null, bool IsDeleted = false)
        {
            try
            {
                var query = _genericAttributeRepository.TableFromBuffer(72);
                if (query != null)
                {
                    query = query.OrderBy(a => a.Sequence).Where(s => s.IsDeleted == IsDeleted);
                    if (!string.IsNullOrEmpty(keyGroup))
                    {
                        query = query.Where(s => s.KeyGroup.Contains(keyGroup.Trim()));
                    }
                    if (query.Any())
                    {
                        return query.OrderBy(a => a.Sequence).OrderByDescending(c => c.Id).Select(s => new GenericAttributeModel()
                        {
                            Author = s.Author,
                            CreateTime = s.CreateTime,
                            Description = s.Description,
                            Id = s.Id,
                            Key = s.Key,
                            KeyGroup = s.KeyGroup,
                            Value = s.Value,
                            Sequence = s.Sequence
                        }
                        ).ToList();
                    }
                }
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetListOfPager");
            }
            return new List<GenericAttributeModel>();
        }
        public IPagedList<GenericAttributeModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, string keyGroup = null, bool IsDeleted = false)
        {
            try
            {
                return new PagedList<GenericAttributeModel>(GetList(keyGroup, IsDeleted), pageIndex, pageSize);
            }
            catch (Exception e)
            {
                _loggerService.insert(e, LogLevel.Warning, "Nav：GetListOfPager");
            }
            return new PagedList<GenericAttributeModel>(new List<GenericAttributeModel>(), pageIndex, pageSize);
        }
    }
}
