using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Common
{
    public class GenericAttributeModel : BaseViewModel
    {
        [Required]
        public string KeyGroup { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
