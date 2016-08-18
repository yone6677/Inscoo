
using System;
using System.Collections.Generic;

namespace Domain
{
    public class MemberInsurance : BaseEntity
    {
        public string UniqueKey { set; get; }
        /// <summary>
        /// 文件类型。
        /// </summary>
        public int FileType { set; get; }
        public string FileTypeName { set; get; }
        public int ExcelId { set; get; }
        public int? EinsuranceId { set; get; }
        /// <summary>
        /// 电子保单号码
        /// </summary>
        public string EOrderCode { set; get; }
        public string Status { set; get; }
        public string PdfFileName { set; get; }

        public virtual FileInfo Excel { set; get; }
        public virtual FileInfo Einsurance { set; get; }

    }
}
