using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyORM.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    class ForeignKey : Attribute
    {
        public bool IsForeignKey { get; set; }
    }
}
