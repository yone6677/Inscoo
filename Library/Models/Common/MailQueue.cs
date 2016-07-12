using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class MailQueue
    {
        public MailQueue()
        {
            MQLSTUPDDATE = DateTime.Now;
        }
        [Key]
        public int MQID { set; get; }
        public string MQTYPE { get; set; }
        public string MQSUBJECT { get; set; }
        public string MQMAILFRM { get; set; }
        public string MQMAILTO { get; set; }
        public string MQMAILCC { get; set; }
        public string MQMAILBCC { get; set; }
        public string MQMAILCONTENT { get; set; }
        public string MQFILE { get; set; }
        public int MQMAILCOUNT { get; set; }
        public DateTime MQLSTUPDDATE { get; set; }
    }
}
