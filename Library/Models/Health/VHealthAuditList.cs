﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace Models
{
    public class VHealthAuditList
    {
        public int MasterId { set; get; }
        [DisplayName("客户")]
        public string Author { set; get; }
        [DisplayName("产品类型")]
        public string ProductTypeName { set; get; }
        public string ProductCode { set; get; }
        [DisplayName("产品名称")]
        public string ProductName { set; get; }

        [DisplayName("订单状态")]
        public decimal Status { set; get; }

        [DisplayName("订单状态")]
        public string StatusDes { set; get; }
        [DisplayName("优惠价")]
        public decimal PrivilegePrice { set; get; }


    }
}