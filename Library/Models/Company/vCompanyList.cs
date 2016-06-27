using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public class vCompanyList
    {
        public int Id { set; get; }

        [DisplayName("企业名称")]
        public string Name { set; get; }

        [DisplayName("企业地址")]
        public string Address { set; get; }

        [DisplayName("联系人")]
        public string LinkMan { set; get; }

        [DisplayName("联系电话")]
        public string Phone { set; get; }

        [DisplayName("邮箱")]
        public string Email { set; get; }


        [DisplayName("企业代码")]
        public string Code { set; get; }

        [DisplayName("营业执照")]
        public string BusinessLicenseFilePath { set; get; }
    }
}
