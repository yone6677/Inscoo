using System.ComponentModel;

namespace Models
{
    public class vProvisionPDF
    {
        [DisplayName("保险公司")]
        public string InsuredCom { get; set; }
        [DisplayName("保险方案")]
        public string SafeguardName { get; set; }

        [DisplayName("条款")]
        public string ProvisionPath { get; set; }
    }
}
