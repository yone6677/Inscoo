using System.Collections.Generic;

namespace Services.Infrastructure
{
    public class ExcelModel<T>
    {
        /// <summary>
        /// Sheet名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// 列数
        /// </summary>
        public int ColumnCount { get; set; }
        /// <summary>
        /// 列名
        /// </summary>
        public List<string> ColumnName { get; set; }
        /// <summary>
        /// 待填充数据
        /// </summary>
        public List<T> Table { get; set; }
        /// <summary>
        /// 是否激活此Sheet为主页
        /// </summary>
        public bool IsActive { get; set; }
    }
}
