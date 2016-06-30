﻿using Core.Pager;
using Domain.Common;
using Models.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Services
{
    public interface IGenericAttributeService
    {
        bool Insert(GenericAttribute item);
        bool Update(GenericAttribute item);
        bool Delete(int id, bool disable = true);
        List<GenericAttribute> GetByGroup(string keyGroup);
        SelectList GetSelectListByGroup(string keyGroup, string selectedValue);
        GenericAttribute GetById(int id);
        GenericAttribute GetByKey(string key = null, string keyGroup = null, string value = null);
        List<SelectListItem> GetSelectList(string keyGroup = null, bool isTip = false, bool IsDeleted = false);
        List<GenericAttributeModel> GetList(string keyGroup = null, bool IsDeleted = false);
        IPagedList<GenericAttributeModel> GetListOfPager(int pageIndex = 1, int pageSize = 15, string keyGroup = null, bool IsDeleted = false);
    }
}
