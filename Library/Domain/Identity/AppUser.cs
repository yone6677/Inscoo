using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Domain

{
    public partial class AppUser : IdentityUser
    {
        public AppUser()
        {
            CreateTime = DateTime.Now;
            ModifyTime = DateTime.Now;
            IsDelete = false;
            TiYong = false;
            FanBao = false;
        }
        public bool IsDelete { set; get; }
        public DateTime CreateTime { set; get; }
        public DateTime ModifyTime { set; get; }
        public string CreaterId { set; get; }
        public string Changer { set; get; }
        public string CompanyName { set; get; }
        public string LinkMan { set; get; }
        public bool TiYong { set; get; }
        public bool FanBao { set; get; }
        public int Ident { get; set; }
        //返点分配
        public int Rebate { get; set; }

        //户名
        public string AccountName { get; set; }
        //开户行
        public string BankName { get; set; }
        //开户行账号
        public string BankNumber { get; set; }

        //佣金计算方法
        public string CommissionMethod { get; set; }
    }
}
