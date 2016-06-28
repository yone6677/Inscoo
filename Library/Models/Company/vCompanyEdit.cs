using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public class vCompanyEdit
    {
        public int Id { set; get; }

        [DisplayName("企业名称")]
        [MaxLength(100)]
        [Required(ErrorMessage = "请输入企业名称")]
        public string Name { set; get; }

        [DisplayName("企业地址")]
        [MaxLength(100)]
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "请输入企业地址")]
        public string Address { set; get; }

        [DisplayName("联系人")]
        [MaxLength(100)]
        [Required(ErrorMessage = "请输入联系人姓名")]
        public string LinkMan { set; get; }

        [DisplayName("联系电话")]
        [MaxLength(30)]
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "请输入正确联系电话")]
        public string Phone { set; get; }

        [DisplayName("邮箱")]
        [MaxLength(100)]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "请输入正确邮箱")]
        public string Email { set; get; }


        [DisplayName("企业代码")]
        [Editable(false)]
        public string Code { set; get; }

        [DisplayName("营业执照")]
        public string BusinessLicenseFilePath { set; get; }
    }
}
