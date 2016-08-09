using System;
using Models.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Models
{
    public class CarInsuranceDetailSearchModel
    {

        [DisplayName("投保人")]
        public string InsuredName { set; get; }

        [DisplayName("车牌")]
        public string InsuredCarNo { set; get; }
    }
}
