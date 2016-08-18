
using System;

namespace Domain
{
    public class MemberInsuranceDetail : BaseEntity
    {
        public int DetailID { get; set; }
        public int CarInsuranceID { get; set; }
        public string InsuredDoc { get; set; }
        public string InsuredName { get; set; }
        public string InsuredCarNo { get; set; }
        public string InsuredCarID { get; set; }
        public string InsuredCompanyID { get; set; }
        public string InsuredTel { get; set; }
        public int InsuredSetNumber { get; set; }
        public int InsuredExpense { get; set; }
        public DateTime? InsuredBeginDate { get; set; }
        public string InsuredMedicalFee { get; set; }
        public string InsurancePolicy { get; set; }
        public DateTime? InsuredEndingDate { get; set; }
        public DateTime? InsuredInsertDate { get; set; }
        public int IsDelete { set; get; }
    }
}
