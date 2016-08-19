using System;
using Models.Common;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace Models
{
    public class vMemberInsuranceList
    {
        public int InsuranceId { set; get; }
        public int ExcelId { set; get; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string ExcelName { get; set; }
        public string Status { get; set; }

        public string UniqueKey { get; set; }

        public string EOrderCode { get; set; }


        public string ExceUrl { get; set; }
        public string PdfFileName { get; set; }
        public DateTime ExceCreateTime { set; get; }
        public string Author { set; get; }
        public string FileTypeName { set; get; }
        public int FileType { set; get; }
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
