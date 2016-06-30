using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MixProductModel : BaseViewModel
    {
        [DisplayName("名称")]
        [Required]
        public string Name { get; set; }
        [StringLength(10, ErrorMessage = "{0}必须为{2} 个字符。", MinimumLength = 10)]
        [DisplayName("产品编码")]
        [Required]
        public string Code { get; set; }
        [DisplayName("描述")]
        public string Description { get; set; }
        [DisplayName("价格")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }
        [DisplayName("原价")]
        public decimal OriginalPrice { get; set; }
        public string Address { get; set; }
        [DisplayName("适用人数")]
        [Required]
        public string StaffRange { get; set; }
        [DisplayName("参保年龄")]
        public string AgeRange { get; set; }
    }
}
