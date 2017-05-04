using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassGenerator
{
    public static class AppHelper
    {
        public static List<String> LoadUsingStatements()
        {
            List<String> list = new List<String>();
            list.Add("using System;");
            list.Add("using System.Collections.Generic;");         
            list.Add("using System.Drawing;");
            list.Add("using System.Text;");
            return list;
        }
    }
}
