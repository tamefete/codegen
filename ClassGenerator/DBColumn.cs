using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGenerator
{
    public class DBColumn
    {
        public String Name { get; set; }
        public Boolean IsNullable { get; set; }
        public Type DataType { get; set; }
    }
}
