using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract partial class BaseApiModel
    {
        public virtual int Id { get; set; }
    }
}
