
using System;

namespace Domain
{
    public class CarInsuranceFile : BaseEntity
    {
        public CarInsuranceFile()
        {
            EditTime = DateTime.Now;
            Type = 1;
        }

        /// <summary>
        /// excel 1  Einsurance 2
        /// </summary>
        public int Type { get; set; }
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

        public int CarInsuranceId { set; get; }
        public DateTime EditTime { set; get; }
        public virtual CarInsurance CarInsurance { set; get; }
    }
}
