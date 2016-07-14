using System;
using Models.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Models
{
    public class vCarInsuranceList
    {
        public int InsuranceId { set; get; }
        public int ExcelId { set; get; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string ExcelName { get; set; }

        public string ExceUrl { get; set; }
        public DateTime ExceCreateTime { set; get; }
        public string ExceCreateUser { set; get; }
        /// <summary>
        /// 电子保单地址
        /// </summary>
        public string EinsuranceUrl { set; get; }
        /// <summary>
        /// 电子保单名称
        /// </summary>
        public string EinsuranceName { set; get; }
    }
}
