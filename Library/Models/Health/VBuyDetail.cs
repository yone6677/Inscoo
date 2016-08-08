using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    /// <summary>
    /// 订单信息
    /// </summary>
    public class VBuyDetail
    {
        public VBuyDetail()
        {
            Count = 1;
        }
        [Required]
        public int ProductId { set; get; }

        [Required]
        [RegularExpression(@"\d+", ErrorMessage = "请输入正确的数量")]
        public int Count { get; set; }

        public string ProductName { get; set; }
        public string ProductType { get; set; }
    }
}