using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Permission
{
    public class PermissionModel : BaseViewModel
    {
        public string rid { get; set; }
        [DisplayName("角色")]
        public List<SelectListItem> roles { get; set; }
        [DisplayName("功能")]
        public string pid { get; set; }
    }
}
