
using System;
using System.Collections.Generic;

namespace Domain
{
    public class CarInsurance : BaseEntity
    {

        public string AppUserId { set; get; }

        public virtual AppUser AppUser { set; get; }

        public virtual IList<CarInsuranceFile> CarInsuranceFiles { set; get; }
        //public virtual IList<CarInsuranceEinsurance> CarInsuranceEinsurances { set; get; }
    }
}
