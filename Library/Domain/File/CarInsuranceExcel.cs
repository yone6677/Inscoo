
using System;

namespace Domain
{
    public class CarInsuranceExcel : BaseFile
    {
        public CarInsuranceExcel()
        {
            EinsuranceEditTime = DateTime.Now;
        }
        public string AppUserId { set; get; }
        public DateTime EinsuranceEditTime { set; get; }
        /// <summary>
        /// 电子保单目录
        /// </summary>
        public string EinsurancePath { set; get; }
        /// <summary>
        /// 电子保单地址
        /// </summary>
        public string EinsuranceUrl { set; get; }
        /// <summary>
        /// 电子保单名称
        /// </summary>
        public string EinsuranceName { set; get; }
        public virtual AppUser AppUser { set; get; }
    }
}
