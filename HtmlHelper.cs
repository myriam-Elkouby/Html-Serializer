using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;

namespace Html_Serializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        public static HtmlHelper Instance => _instance;
        private HtmlHelper()
        {
            // load information from Json files

            var jsonContent = File.ReadAllText(@"Json/HtmlTags.json");

            HtmlTags = JsonSerializer.Deserialize<string[]>(jsonContent);

            jsonContent = File.ReadAllText(@"Json/HtmlVoidTags.json");

            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(jsonContent);

        }    

    }
}
