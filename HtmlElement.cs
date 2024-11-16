using Html_Serializer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Html_Serializer
{
    public class HtmlElement
    {

        public HtmlElement()
        {
            Attributes = new HashSet<HtmlAttribute>();
            Classes = new List<string>();
            Parent = null;
            Children = new List<HtmlElement>();
        }
        public HtmlElement(string name, HashSet<HtmlAttribute> attributes)
        {
            Name = name;
            Attributes = new HashSet<HtmlAttribute>();
            Classes = new List<string>();
            foreach(var attribute in attributes)
            {
                if (attribute.Name == "class")
                {
                    string[] classes = attribute.Value.Split(' ');
                    foreach(string c in classes)
                    {
                        Classes.Add(c);
                    }                  
                }                   
                else if (attribute.Name == "id")
                    Id = attribute.Value;
                else Attributes.Add(attribute);

            }
            Parent = null;
            Children = new List<HtmlElement>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public HashSet<HtmlAttribute> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }


        public IEnumerable<HtmlElement> Descendants()
        {
            
            Queue<HtmlElement> treeElements = new Queue<HtmlElement>();
            treeElements.Enqueue(this);
            while (treeElements.Count > 0) 
            {
                HtmlElement first = treeElements.Dequeue();
    
                // yield return - created object by request
                yield return first;

                foreach (var child in first.Children)
                {
                    treeElements.Enqueue(child);
                }

            }            
        }

        
        public List<HtmlElement> Ancestors()
        {
            List<HtmlElement> ancestors = new List<HtmlElement>();
            HtmlElement current = this;
            while (current != null)
            {
                ancestors.Add(current);
                current = current.Parent;
            }
            return ancestors;
        }

        public override string ToString()
        {
            string classes = string.Join(" ", Classes);
            string atributes = string.Empty;
            foreach (var atribute in Attributes)
            {
                atributes += atribute.Name +"="+ atribute.Value;
            }
            return $"Name: {Name}, Id: {Id}, Classes: {classes}, Attributes: {atributes}, InnerHtml: {InnerHtml}";
        }
    }
}

public static class HtmlElementExtention
{
    public static bool isMatch(Selector selector, HtmlElement element)
    {
        if (selector.TagName != string.Empty)
        {
            if (element.Name != selector.TagName)
                return false;
        }
        if(selector.Id != string.Empty)
            if (element.Id != selector.Id)
                return false;
        foreach (var one in selector.Classes)
        {
            string classes = string.Join("\"", element.Classes); 
            Console.WriteLine("+"+ classes.Contains(one));
            if (!classes.Contains(one))
                return false;
        }
        return true;
    }
    public static void ReqFunc(Selector selector, HashSet<HtmlElement> resSearch, HtmlElement current)
    {
        // stop 
        if (selector == null)
        {
            resSearch.Add(current);
            return;
        }
        var Descendants = current.Descendants();
        Descendants = Descendants.Where((element) => isMatch(selector, element));
        
        foreach(var descendant in Descendants)
        {
            ReqFunc(selector.Child, resSearch, descendant);
        }
    }
    public static HashSet<HtmlElement> FindBySelector(this HtmlElement currentElement, Selector selector)
    {
        HashSet<HtmlElement> htmlElements = new HashSet<HtmlElement>();
        ReqFunc(selector, htmlElements, currentElement);
        return htmlElements;
    }
}

