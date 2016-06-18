using System.Collections.Generic;

namespace Domain.Products
{
    public class MixProduct : BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }      
        public string Address { get; set; }
        public string StaffRange { get; set; }
        public string AgeRange { get; set; }
        public virtual ICollection<MixProductItem> ProductMixItem { get; set; }
    }
}
