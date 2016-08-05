using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    public class WZSearchModel
    {
        public string UserId { set; get; }

        [DisplayName("公司名称")]
        public string CompanyName { set; get; }

        public string Author { set; get; }

    }
}
