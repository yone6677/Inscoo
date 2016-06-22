using Microsoft.AspNet.Identity.EntityFramework;
using System;

namespace Core
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
    }
}
