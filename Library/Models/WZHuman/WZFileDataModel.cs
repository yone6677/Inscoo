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
    /// 保险人员名单列表
    /// </summary>
    public class WZFileDataModel
    {
        [DisplayName("文件名")]
        public string FileName { set; get; }
        [DisplayName("上传人")]
        public string Author { set; get; }
        [DisplayName("上传日期")]
        public DateTime UploadDate { set; get; }
        [DisplayName("备注")]
        public string Memo { set; get; }
        public string Url { set; get; }
    }
}
