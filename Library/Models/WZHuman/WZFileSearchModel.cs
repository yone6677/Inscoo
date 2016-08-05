using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Models
{
    /// <summary>
    /// 查询保险人员上传名单
    /// </summary>
    public class WZFileSearchModel
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { set; get; }
        /// <summary>
        /// 上传人
        /// </summary>
        public string Author { set; get; }
        /// <summary>
        /// 上传日期
        /// </summary>
        public DateTime? CreateTime { set; get; }

        public int MasterId { set; get; }
    }
}
