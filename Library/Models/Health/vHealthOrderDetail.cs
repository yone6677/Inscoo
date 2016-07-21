using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Models
{
    public class VHealthOrderDetail
    {
        public string Name { set; get; }
        public string Sex { set; get; }
        public DateTime? Birthday { set; get; }

        /// <summary>
        /// 证件号码
        /// </summary>
        public string IdNumber { set; get; }
        /// <summary>
        /// 婚姻状况
        /// </summary>
        public string Marriage { set; get; }

        /// <summary>
        ///移动电话
        /// </summary>
        public string Phone { set; get; }
        public string Email { set; get; }
        /// <summary>
        /// 所在城市
        /// </summary>
        public string Address { set; get; }
        /// <summary>
        /// 公司名称(Excel上传，可以为空)
        /// </summary>
        public string CompanyName { set; get; }

        /// <summary>
        /// 部门
        /// </summary>
        public string DepartMent { set; get; }
        /// <summary>
        /// 职位
        /// </summary>
        public string Chair { set; get; }
      
    }
}
