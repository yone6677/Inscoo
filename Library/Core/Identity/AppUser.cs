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
            Rebate = 0;
        }
        public bool IsDelete { set; get; }
        public DateTime CreateTime { set; get; }
        public DateTime ModifyTime { set; get; }
        public string CreaterId { set; get; }
        public string Changer { set; get; }
        public string CompanyName { set; get; }
        public string LinkMan { set; get; }
        /// <summary>
        /// 利润加成
        /// </summary>
        public bool TiYong { set; get; }
        /// <summary>
        /// 理赔比例
        /// </summary>
        public bool FanBao { set; get; }
        public int Ident { get; set; }
        /// <summary>
        /// 返点
        /// </summary>
        public int Rebate { get; set; }
    }
}
