using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeScanner_GUI_NF
{
    public class Token
    {
        public string value;
        public string type;
        public Token(string value, string type)
        {
            this.value = value;
            this.type = type;
        }
    }
}
