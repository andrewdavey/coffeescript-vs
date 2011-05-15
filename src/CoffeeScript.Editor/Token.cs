using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeScript.Editor
{
    public class Token
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public int Index { get; set; }
    }
}
