using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public class vCompanySearch
    {
        public string UserId { set; get; }
        public string Name { set; get; }

        public string Address { set; get; }

        public string LinkMan { set; get; }

        public string Phone { set; get; }

        public string Email { set; get; }


        public string Code { set; get; }
    }
}
