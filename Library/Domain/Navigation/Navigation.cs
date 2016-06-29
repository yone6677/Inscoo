using System.Collections.Generic;

namespace Domain
{
    public partial class Navigation : BaseEntity
    {
        public string name { get; set; }
        public int level { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
        public string url { get; set; }
        public int pId { get; set; }
        public bool isShow { get; set; }
        public string memo { get; set; }
        public string htmlAtt { get; set; }
        public int sequence { get; set; }
        public virtual List<Navigation> SonMenu { get; set; }
    }
}
