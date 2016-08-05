using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Domain
{
    public class WZHumanMaster : BaseEntity
    {
        /// <summary>
        /// 登陆账号
        /// </summary>
        public string Account { set; get; }
        /// <summary>
        /// 客户公司名称
        /// </summary>
        public string CompanyName { set; get; }

        /// <summary>
        /// 保险生效开始日期
        /// </summary>
        public DateTime? InsuranceBeginTime { set; get; }
        /// <summary>
        /// 保险生效结束日期
        /// </summary>
        public DateTime? InsuranceEndTime { set; get; }

    }
}
