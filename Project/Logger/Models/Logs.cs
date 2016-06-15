using System;

namespace Logger.Models
{
    public class Logs
    {
        public int Id { get; set; }
        public string Uid { get; set; }
        public int Level { get; set; }
        public int HttpStatusCode { get; set; }
        public int HResult { get; set; }
        public string Message { get; set; }
        public string Parameters { get; set; }
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Ip { get; set; }
        public string Browser { get; set; }
        public string Memo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}