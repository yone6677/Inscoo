using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class HealthFile : BaseEntity
    {
        /// <summary>
        /// 文件ID
        /// </summary>
        public int FId { get; set; }
        /// <summary>
        /// 体检订单ID
        /// </summary>
        public int HId { get; set; }
        public virtual HealthOrderMaster order { get; set; }
    }
}
