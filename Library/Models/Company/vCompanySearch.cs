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

        [DisplayName("企业名称")]
        public string Name { set; get; }

        [DisplayName("企业地址")]
        public string Address { set; get; }

    }
}
