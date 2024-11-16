using Html_Serializer;
using System;
using System.Text.RegularExpressions;

//https://www.jobmaster.co.il/
//file:///F:/%D7%A4%D7%A8%D7%A7%D7%98%D7%99%D7%A7%D7%95%D7%93/Html%20Serializer/js/index.html
//https://moodle.malkabruk.co.il/mod/assign/view.php?id=102
HtmlElement dom = await HtmlSerializer.Serialize("https://netfree.link/app/#/tickets/new?u=https:%2F%2Fmedium.com%2Fdeveloper-student-clubs-tiet%2Fcreating-shell-command-for-formatting-files-in-newly-created-create-react-app-c2e74ad488c0&r=https:%2F%2Fwww.google.com%2F&t=site&bi=");


Selector selector1 = Selector.ConvertQueryToSelector("script");
var res1 = dom.FindBySelector(selector1);
Console.WriteLine("Find By Selector 1:");
res1.ToList().ForEach(e => Console.WriteLine("res1: " + e.ToString()));

Selector selector2 = Selector.ConvertQueryToSelector("head link");
var res2 = dom.FindBySelector(selector2);
Console.WriteLine("Find By Selector 2:");
res2.ToList().ForEach(e => Console.WriteLine("res2: " + e.ToString()));

Selector selector3 = Selector.ConvertQueryToSelector("head link#favIcon");
var res3 = dom.FindBySelector(selector3);
Console.WriteLine("Find By Selector 3:");
res3.ToList().ForEach(e => Console.WriteLine("res3: " + e.ToString()));


Console.WriteLine("Finish");



