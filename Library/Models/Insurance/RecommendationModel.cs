using Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models.Insurance
{
    public class RecommendationModel : BaseViewModel
    {
        public RecommendationModel()
        {
            pids = new List<int>();
        }
        [DisplayName("名称")]
        [Required]
        public string Name { get; set; }
        [DisplayName("价格")]
        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }
        public string Address { get; set; }
        [DisplayName("适用人数")]
        [Required]
        public string StaffRange { get; set; }
        [DisplayName("参保年龄")]
        public string AgeRange { get; set; }
        public List<int> pids { get; set; }
        public List<MixProductItemModel> item { get; set; }
    }
}
