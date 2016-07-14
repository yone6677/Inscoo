using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MixProductItemModel : BaseViewModel
    {
        public int mid { get; set; }
        [DisplayName("保障范围")]
        public string SafefuardName { get; set; }
        [DisplayName("原始价格")]
        [DataType(DataType.Currency)]
        public decimal OriginalPrice { get; set; }
        [DisplayName("保障额度")]
        public string CoverageSum { get; set; }

        public string ProdInsuredName { set; get; }

        public string ProdMemo { set; get; }
        public string ProvisionPath { get; set; }
    }
}
