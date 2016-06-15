using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Models.Navigation
{
    public class NavigationViewModel: BaseViewModel
    {
        [Required]
        [Display(Name = "名称")]
        public string name { get; set; }
        [Display(Name = "级别")]
        public int level { get; set; }
        [Display(Name = "Controller")]
        public string controller { get; set; }
        [Display(Name = "Action")]
        public string action { get; set; }
        [Display(Name = "Url")]
        public string url { get; set; }
        public int pId { get; set; }
        [Required]
        [Display(Name = "是否展示")]
        public bool isShow { get; set; }
        [Display(Name = "备注")]
        public string memo { get; set; }
        [Display(Name = "HTML属性")]
        public string htmlAtt { get; set; }
        [Display(Name = "展示顺序")]
        public int sequence { get; set; }
        public List<NavigationViewModel> SonMenu { get; set; }
       //public NavigationViewModel parMenu { get; set; }
        public string permissionList { get; set; }
        public bool hasPermission { get; set; }
    }
}
