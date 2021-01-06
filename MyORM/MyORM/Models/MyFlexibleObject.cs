using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyORM
{
    public class MyFlexibleObject
    {
        private Dictionary<string, object> attributes = new Dictionary<string, object>();

        public object this[string strAttributeName]
        {
            get
            {
                return attributes[strAttributeName];
            }
            set
            {
                attributes.Add(strAttributeName, value);
            }
        }
    }
}