using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Cart
{
    public class CartModel : BaseViewModel
    {
        public string UserId { get; set; }
        public DateTime CreateTime { get; set; }
        [Display(Name = "单价")]
        public decimal Price { get; set; }
        [Display(Name = "数量")]
        public int Total { get; set; }
        [Display(Name = "小计")]
        public decimal Amount { get; set; }
        [Display(Name ="商品名称")]
        public string PName { get; set; }
        [Display(Name = "体检机构")]
        public string CompanyName { get; set; }
    }
}
