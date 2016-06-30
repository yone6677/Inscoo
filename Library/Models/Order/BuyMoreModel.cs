using System;
using System.ComponentModel;

namespace Models.Order
{
   public class BuyMoreModel:BaseViewModel
    {
        [DisplayName("生效日期")]
        public DateTime StartDate { get; set; }
        [DisplayName("人员信息模板")]
        public string EmpInfoFileUrl { get; set; }

        public string Result { get; set; }
    }
}
