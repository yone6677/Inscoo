using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Api.Archive
{
    public class DownLoadWechatFileApi : BaseApiModel
    {
        /// <summary>
        /// 下载地址
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 微信Media_Id
        /// </summary>
        public string MediaId { get; set; }
        /// <summary>
        /// 下载类型
        /// </summary>
        public string MediaType { get; set; }
    }
}
