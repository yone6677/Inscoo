
using System;

namespace Domain
{
    public class FileInfo : BaseEntity
    {
        public FileInfo()
        {
            EditTime = DateTime.Now;
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        public string Url { get; set; }

        public DateTime EditTime { set; get; }
    }
}
