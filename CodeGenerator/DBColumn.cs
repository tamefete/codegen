using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    public class DBColumn
    {
        public String Name { get; set; }
        public String DataType { get; set; }
        public Boolean IsNullable { get; set; }
        public Boolean IsIdentity { get; set; }
        public Object DataTypeLength { get; set; }
        public Boolean IsForeignKey { get; set; }
        public Boolean IsPrimaryKey { get; set; }
    }
}
