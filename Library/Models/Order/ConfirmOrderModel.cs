using Models.Products;
using Models.Role;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        [DisplayName("投保员工数")]
        public string StaffRange { get; set; }
        [DisplayName("投保员工平均年龄")]
        public string AgeRange { get; set; }
        [DisplayName("保费（人/年）")]
        public string AnnualExpense { get; set; }
        [DisplayName("产品信息")]
        public List<ProductModel> ProdItem { get; set; }
        public List<UserRoleModel> UserRole { get; set; }
    }
}
