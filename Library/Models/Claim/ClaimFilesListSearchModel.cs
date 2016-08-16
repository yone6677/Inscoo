
using System;
using System.ComponentModel;
using System.Web.WebPages;

namespace Models
{
    public class ClaimFilesListSearchModel
    {
        [DisplayName("上传日期")]
        public DateTime? CreateDate { get; set; }

        [DisplayName("文件名")]
        public string FileName { get; set; }

        [DisplayName("批次")]
        public string BatchCode { get; set; }

        [DisplayName("上传人")]
        public string Author { get; set; }
    }
}
