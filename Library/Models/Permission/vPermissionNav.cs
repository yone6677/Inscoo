using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Models.Permission
{
    public class vPermissionNav : BaseViewModel
    {
        public string Controller { set; get; }

        public string Action { set; get; }

        [DisplayName("菜单名称")]
        public string Name { set; get; }

        [DisplayName("是否分配")]
        public bool IsUsed { set; get; }
    }
}
