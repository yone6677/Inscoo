using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Models.Insurance
{
    public class ProductListModel : BaseViewModel
    {
        public ProductListModel()
        {
            PayoutRatioList = new List<SelectListItem>();
            CoverageSumList = new List<SelectListItem>();
        }
        [DisplayName("公司名称")]
        public string InsuredCom { set; get; }
        public bool Selected { get; set; }
        [DisplayName("保障项目")]
        public string SafeguardName { set; get; }
        [DisplayName("保障项目")]
        public string SafeguardCode { set; get; }
        [DisplayName("保额")]
        public string CoverageSum { get; set; }
        [DisplayName("赔付比例(住院/门诊)")]
        public string PayoutRatio { get; set; }
        [DisplayName("保费")]
        public decimal Price { get; set; }
        public List<SelectListItem> CoverageSumList { get; set; }
        public List<SelectListItem> PayoutRatioList { get; set; }
    }
}
