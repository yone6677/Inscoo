using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class CompanyInfo : BaseEntity
    {

        public string Name { set; get; }
        public string Code { set; get; }
        public string BusinessLicenseFilePath { set; get; }
        public int? BusinessLicenseFileId { set; get; }
        public string Address { set; get; }
        public string LinkMan { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }

        public string UserId { set; get; }

        //public virtual AppUser User { set; get; }
        //public virtual FileUpLoad BusinessLicenseFile { set; get; }
    }
}
