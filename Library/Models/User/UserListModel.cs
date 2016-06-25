using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Models.User
{
    public class UserListModel
    {
        public UserListModel()
        {
            RoleId = "";
            UserName = "";
        }
        [DisplayName("用户名称")]
        public string UserName { set; get; }
        [DisplayName("角色名称")]
        public string Role { set; get; }
        [DisplayName("角色ID")]
        public string RoleId { set; get; }

        [DisplayName("角色列表")]
        public List<SelectListItem> RoleList { get; set; }

        [DisplayName("用户列表")]
        public List<UserModel> UserList { get; set; }

        public GridView UserListGV { set; get; }
    }
}
