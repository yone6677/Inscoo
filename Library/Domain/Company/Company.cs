using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class Company : BaseEntity
    {
        public Company()
        {
            EditTime = DateTime.Now;
        }

        public string Name { set; get; }
        public string Code { set; get; }
        public string Address { set; get; }
        public string LinkMan { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }

        public string UserId { set; get; }

        public DateTime EditTime { set; get; }

        public virtual AppUser User { set; get; }
        public virtual IList<BusinessLicense> BusinessLicenses { set; get; }
    }
}
