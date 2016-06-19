using Models.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Models.Insurance
{
    public class CustomProductModel : BaseViewModel
    {
        [DisplayName("投保员工人数")]
        public List<SelectListItem> StaffsNumber { get; set; }
        [DisplayName("员工平均年龄")]
        public List<SelectListItem> Avarage { get; set; }
        public List<GenericAttributeModel> CompanyList { get; set; }
    }
}
