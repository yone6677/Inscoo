using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class VCheckProductList
    {
        public int Id { set; get; }
        public string ProductTypeName { set; get; }
        public string ProductCode { set; get; }
        public string ProductName { set; get; }
        public string ProductMemo { set; get; }
        public string CompanyCode { set; get; }
        public string CompanyName { set; get; }
        [DisplayName("市场价")]
        public decimal PublicPrice { set; get; }
        [DisplayName("优惠价")]
        public decimal PrivilegePrice { set; get; }

        public string CheckProductPic { set; get; }

    }
}
