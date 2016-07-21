using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Models
{
    public class VCheckProductDetail
    {
        public int Id { set; get; }
        public int MasterId { set; get; }
        [DisplayName("产品类型")]
        public string ProductType { set; get; }
        public string ProductCode { set; get; }
        [DisplayName("产品名称")]
        public string ProductName { set; get; }
        public string ProductMemo { set; get; }
        [DisplayName("市场价")]
        public decimal PublicPrice { set; get; }
        [DisplayName("优惠后价格")]
        public decimal PrivilegePrice { set; get; }

        public string CheckProductPic { set; get; }

    }
}
