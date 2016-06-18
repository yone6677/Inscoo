
using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public class GenericAttribute : BaseEntity
    {
        [Required]
        public string KeyGroup { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
