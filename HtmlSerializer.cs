using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Html_Serializer
{
    public class HtmlSerializer
    {
        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }
        public static HashSet<HtmlAttribute> GetAttributes(string htmlElement)
        {
            var attributes = new Regex("([^\\s].*?)=\"(.*?)\"").Matches(htmlElement);
            var attributesSet = new HashSet<HtmlAttribute>();
            foreach(var attribute in attributes)
            {
                var splitedAttribute = attribute?.ToString()?.Split("=");
                attributesSet.Add(new HtmlAttribute(splitedAttribute?[0], splitedAttribute?[1]));
            }
            return attributesSet;
        }
        public static string GetName(string htmlElement)
        {
            int spaceIndex = htmlElement.IndexOf(' ');
            string name = spaceIndex!= -1 ? htmlElement.Substring(0, spaceIndex): htmlElement;
            return name;
        }
        public static async Task<HtmlElement> Serialize(string url)
        {
            var html = await Load(url);
            HtmlHelper jsonFiles = HtmlHelper.Instance;

            //<tag>
            var htmlLines = new Regex("<(.*?)>").Split(html).Where(s => s.Length > 0);
            // all type of spases \n \r
            htmlLines = htmlLines.Where(line => new Regex("(\\s)").Replace(line, "") != string.Empty);

            var root = htmlLines.First(l => l.StartsWith("html"));
            var name = GetName(root);
            var htmlAttributes = GetAttributes(root.Substring(name.Length));
            HtmlElement rootElement = new HtmlElement(name, htmlAttributes);
            HtmlElement element = new HtmlElement();
            element = rootElement;
            
            foreach (var line in htmlLines)
            {
                var firstWord = GetName(line);

                if (line.StartsWith("html") || line.StartsWith("!DOCTYPE"))
                    continue;
                else if (line.StartsWith("/html"))
                    break;
                else if (line.StartsWith("/"))
                {
                    if(element != null)
                    {
                        element = element.Parent;
                        continue;
                    }
                }
                else if (jsonFiles.HtmlTags.Contains(firstWord) || jsonFiles.HtmlVoidTags.Contains(firstWord))
                {
                    // Attributes
                    htmlAttributes = GetAttributes(line.Substring(firstWord.Length));
                    
                    HtmlElement newElement = new(firstWord, htmlAttributes);
                    // Children
                    if (element != null)
                    {
                        element.Children.Add(newElement);
                        // Parent
                        newElement.Parent = element;
                    }                      
                    if (!jsonFiles.HtmlVoidTags.Contains(name) && !line.EndsWith("/"))
                    {
                        element = newElement;
                    }
                }
                // InnerHtml
                else element.InnerHtml = line;     
            }
            return rootElement;

        }
    }
}
