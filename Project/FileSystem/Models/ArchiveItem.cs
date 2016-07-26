using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileSystem.Models
{
    public class ArchiveItem
    {
        public ArchiveItem()
        {
            CreateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public string Author { get; set; }
        public DateTime CreateTime { set; get; }
        public bool IsDeleted { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 所属单号
        /// </summary>
        public int pId { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 域名
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// 是否来自文件系统
        /// </summary>
        public bool FromAPi { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Memo { get; set; }
        public string Url { get; set; }
        public string Discriminator { get; set; }
    }
}