
namespace Domain
{
    public class BaseFile : BaseEntity
    {
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
    }
}
