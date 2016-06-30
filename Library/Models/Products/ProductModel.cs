﻿using System.ComponentModel;

namespace Models
{
    public class ProductModel : BaseViewModel
    {
        [DisplayName("产品类别")]
        public string ProdType { set; get; }
        [DisplayName("保障方案代码")]
        public string SafeguardCode { set; get; }
        [DisplayName("保障方案名称")]
        public string SafeguardName { set; get; }
        [DisplayName("被保险人类型")]
        public string InsuredWho { set; get; }
        [DisplayName("保额")]
        public string CoverageSum { set; get; }
        [DisplayName("赔付比例")]
        public string PayoutRatio { set; get; }
        [DisplayName("售价")]
        public string Price { get; set; }
        [DisplayName("原价")]
        public string OriginalPrice { get; set; }
    }
}
