using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Models
{
    public class TRegisterModel
    {
        public TRegisterModel()
        {
            EncryBeginDate = DateTime.Now;
        }
        public int Id { set; get; }

        [DisplayName("生效开始日期")]
        public DateTime? EncryBeginDate { set; get; }
        [DisplayName("生效结束日期")]
        public DateTime? EncryEndDate { set; get; }

        [Display(Name = "企业名称")]
        [MaxLength(120, ErrorMessage = "请检查输入")]
        public string CompanyName { get; set; }
        [Display(Name = "角色")]
        public string Roles { get; set; }


        [Display(Name = "返点分配")]
        public int Rebate { set; get; }

        [Display(Name = "利润加成")]
        public bool TiYong { set; get; }
        [Display(Name = "理赔比率")]
        public bool FanBao { set; get; }

        [Display(Name = "是否停用")]
        public bool IsDelete { set; get; }
        public SelectList RoleSelects { get; set; }
        public SelectList CommissionMethods { get; set; }

        [Display(Name = "自选产品权限(保险公司)")]
        public string[] ProdInsurances { get; set; }

        [Display(Name = "专属产品权限")]
        public string[] ProdSeries { get; set; }

        [Display(Name = "佣金计算方法")]
        public string CommissionMethod { get; set; }

        [Display(Name = "备注")]
        [MaxLength(200, ErrorMessage = "输入字符太多")]
        public string Memo { get; set; }

        public string Type { get; set; }
    }
}