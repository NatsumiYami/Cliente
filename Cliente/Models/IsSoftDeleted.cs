using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cliente.Models
{
    internal interface IsSoftDeleted
    {
        bool IsDeleted { get; set; }
    }
}
