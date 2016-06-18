
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Inscoo.Models.Role
{
    public class RoleModel
    {
        [DisplayName("名称")]
        [Required]
        public string Name { get; set; }
        [DisplayName("描述")]
        public string Description { set; get; }
    }
}