using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ClaimFilesList : BaseEntity
    {

        public int ClaimFilesListID { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string ClaimFilesBatchCode { get; set; }
        /// <summary>
        /// 理赔文件名称
        /// </summary>
        public string ClaimFilesName { get; set; }
        /// <summary>
        /// 状态，Excel文件上传后，状态为1
        /// </summary>
        public string ClaimFilesStatus { get; set; }

    }
}
