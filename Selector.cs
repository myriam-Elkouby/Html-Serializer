using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Html_Serializer
{
    public class Selector
    {
        public Selector()
        {
            TagName = string.Empty;
            Classes = new List<string>();
            Parent = null;
            Child = null;
            Id = string.Empty;
        }
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }

        //    public static Selector ConvertQueryToSelector(string query)
        //    {
        //        HtmlHelper jsonFiles = HtmlHelper.Instance;
        //        string[] queryLevels = query.Split(' ');
        //        Selector root = new Selector();
        //        Selector current = root;
        //        foreach (string queryLevel in queryLevels)
        //        {
        //            var idIndex = queryLevel.IndexOf('#');
        //            var classIndex = queryLevel.IndexOf('.');

        //            char[] delimiters = { '.', '#' };
        //            string[] lineParts = queryLevel.Split(delimiters);
        //            string tag = lineParts[0];
        //            if (HtmlHelper.isValidTag(tag))
        //            {
        //                current.TagName = tag;
        //            }
        //            if(idIndex != -1)
        //            {
        //                string _id = ((classIndex != -1) && (idIndex > classIndex)) ? lineParts[2] : lineParts[1];
        //                current.Id = _id;
        //            }
        //            if (classIndex != -1)
        //            {
        //                string _class = ((idIndex != -1) && (classIndex > idIndex)) ? lineParts[2] : lineParts[1];
        //                current.Classes.Add(_class);
        //            }

        //            Selector selector = new Selector();
        //            current.Child = selector;
        //            selector.Parent= current;
        //            current = selector;
        //        }
        //        if(current.Parent != null)
        //            current.Parent.Child = null;
        //        return root;
        //    }
        //}

        public override string ToString()
        {
            string classes = string.Join(" ", Classes);
            return $"Name: {TagName}, Id: {Id}, Classes: {classes}";
        }
        public void print()
        {
            Selector iterator = this;
            while(iterator != null)
            {
                Console.WriteLine(iterator.ToString());
                iterator = iterator.Child;
            }
            
        }

        public static Selector ConvertQueryToSelector(string query)
        {
            HtmlHelper jsonFiles = HtmlHelper.Instance;
            string[] queryLevels = query.Split(' ');
            Selector root = new Selector();
            Selector current = root;

            bool classFlag = false;
            bool idFlag = false;
            string context = "";
            char classChar = '.';
            char idChar = '#';
            foreach (string queryLevel in queryLevels)
            {
                int i = 0;
                while (i < queryLevel.Length)
                {
                    if (queryLevel[i] != '.' && queryLevel[i] != '#')
                    {
                        context += queryLevel[i];
                        i++;
                    }
                    else if (idFlag == false && classFlag == false)
                    {
                        if (jsonFiles.HtmlTags.Contains(context))
                        {
                            current.TagName = context;
                        }
                        classFlag = (queryLevel[i] == classChar);
                        idFlag = (queryLevel[i] == idChar);
                        context = "";
                        i++;
                    }
                    else if (idFlag == true)
                    {
                        context = "\"" + context + "\"";
                        current.Id = context;

                        classFlag = (queryLevel[i] == classChar);
                        idFlag = (queryLevel[i] == idChar);
                        context = "";
                        i++;
                    }
                    else if (classFlag == true)
                    {
                        context = "\"" + context + "\"";
                        current.Classes.Add(context);

                        classFlag = (queryLevel[i] == classChar);
                        idFlag = (queryLevel[i] == idChar);
                        context = "";
                        i++;
                    }
                }

                if (idFlag == false && classFlag == false)
                {
                    if (jsonFiles.HtmlTags.Contains(context))
                    {
                        current.TagName = context;
                    }
                    context = "";
                }
                else if (idFlag == true)
                {
                    context = "\"" + context + "\"";
                    current.Id = context;
                    idFlag = false;
                    context = "";
                }
                else if (classFlag == true)
                {
                    context = "\"" + context + "\"";
                    current.Classes.Add(context);
                    classFlag = false;
                    context = "";
                }


                Selector selector = new Selector();
                current.Child = selector;
                selector.Parent = current;
                current = selector;
            }
            if (current.Parent != null)
                current.Parent.Child = null;
            return root;
        }
    
    
    }
}

