
using System;

namespace Domain
{
    public abstract partial class BaseEntity
    {
        protected BaseEntity()
        {
            CreateTime = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreateTime { set; get; }

    }
}
