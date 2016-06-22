using Models.Products;
using Models.Role;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.Order
{
    public class ConfirmOrderModel : BaseViewModel
    {
        public ConfirmOrderModel()
        {
            ProdItem = new List<ProductModel>();
            UserRole = new List<UserRoleModel>();
        }
        [DisplayName("方案名称")]
        [Required]
        public string OrderName { get; set; }
        [DisplayName("产品备注")]
        public string Memo { get; set; }
        [DisplayName("投保员工数")]
        public string StaffRange { get; set; }
        [DisplayName("投保员工平均年龄")]
        public string AgeRange { get; set; }
        [DisplayName("保费（人/年）")]
        public decimal AnnualExpense { get; set; }
        [DisplayName("利润加成")]
        public int TiYong { set; get; }
        [DisplayName("理赔比率")]
        public int FanBao { set; get; }
        [DisplayName("返点（%）")]
        public int Rebate { get; set; }
        /// <summary>
        /// 产品ID集合
        /// </summary>
        public string ids { get; set; }
        /// <summary>
        /// 用户设置的返点
        /// </summary>
        public int UserRebate { get; set; }
        [DisplayName("对外售价")]
        public decimal pretium { get; set; }
        [DisplayName("产品信息")]
        public List<ProductModel> ProdItem { get; set; }
        public List<UserRoleModel> UserRole { get; set; }
    }
}
