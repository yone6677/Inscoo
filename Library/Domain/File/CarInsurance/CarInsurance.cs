
using System;
using System.Collections.Generic;

namespace Domain
{
    public class CarInsurance : BaseEntity
    {

        public string AppUserId { set; get; }
        public int CarInsuranceExcelId { set; get; }
        public int CarInsuranceEinsurancelId { set; get; }
        public virtual AppUser AppUser { set; get; }

        public virtual FileInfo Excel { set; get; }
        public virtual FileInfo Einsurance { set; get; }

    }
}
