using Core.Pager;
using Domain.Common;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Services.Common
{
    public interface IGenericAttributeService
    {
        bool Insert(GenericAttribute item);
        bool Update(GenericAttribute item);
        bool Delete(int id, bool disable = true);
        GenericAttribute GetById(int id);
        List<SelectListItem> GetSelectList(string keyGroup = null);
        IPagedList<GenericAttribute> GetListOfPager(int pageIndex = 1, int pageSize = 15, string keyGroup = null, bool IsDeleted = false);
    }
}
