using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Claim
{
    public class WechatFileModel : BaseViewModel
    {
        public string Url { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string FType { get; set; }
        public int State { get; set; }
    }
}
