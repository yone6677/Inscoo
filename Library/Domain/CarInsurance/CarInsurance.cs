
using System;
using System.Collections.Generic;

namespace Domain
{
    public class CarInsurance : BaseEntity
    {
        public string UniqueKey { set; get; }
        public string AppUserId { set; get; }
        /// <summary>
        /// 文件类型。0车险，
        /// </summary>
        public int FileType { set; get; }
        public int ExcelId { set; get; }
        public int? EinsuranceId { set; get; }
        /// <summary>
        /// 电子保单号码
        /// </summary>
        public string EOrderCode { set; get; }
        public string Status { set; get; }
        public string PdfFileName { set; get; }
        public virtual AppUser AppUser { set; get; }

        public virtual FileInfo Excel { set; get; }
        public virtual FileInfo Einsurance { set; get; }

    }
}
