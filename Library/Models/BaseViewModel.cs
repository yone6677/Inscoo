using System.Collections.Generic;

namespace Models
{
    public abstract partial class BaseViewModel
    {
        public BaseViewModel()
        {
            CustomProperties = new Dictionary<string, object>(); 
        }
        public Dictionary<string, object> CustomProperties { get; set; }
        public virtual int Id { get; set; }
    }
}
