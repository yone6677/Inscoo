using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public class WZListModel
    {
        public int Id { set; get; }

        [DisplayName("登陆账号")]
        public string Account { set; get; }

        [DisplayName("公司名称")]
        public string CompanyName { set; get; }
    }
}
