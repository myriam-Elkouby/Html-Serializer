using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlAttribute
    {
        public HtmlAttribute(string name, string value) 
        {
            Name = name;
            Value = value;
        }
        public string? Name { get; set; }
        public string? Value { get; set; }

    }
}
