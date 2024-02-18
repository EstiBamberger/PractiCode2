using System.Linq;
using System.Text.RegularExpressions;
using course2;
var html = await Load("https://primereact.org/");
var cleanHtml = new Regex("\\s+").Replace(html, " ");//ניקוי ה-html

var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0).ToList();//regex בסיסי שמחלק את ה-html לשורות נפרדות כך שכל שורה תהיה חלק מה-html
var tree = ProcessHtmlLine(htmlLines);
string selector1 = "html haed meta";
string selector2 = "ul.flex.list-none.m-0.p-0.gap-2.align-items-center li";
string selector3 = "div p";

var selector = Selector.ConvertToSelector(selector1);
var elementsList = tree.elementsMeetSelectorsCteria(selector);

Console.WriteLine(elementsList.ToList().Count() + " elements:");
foreach (var element in elementsList)
{
    foreach (var father in element.Ancestors().ToList())
    {
        PrintHtmlElement(father);
    }
    Console.WriteLine();
    PrintHtmlElement(element);
    Console.WriteLine("--------------------------------------------------------");
}
static void PrintHtmlElement(HtmlElement element, string indent = "")
{
    Console.Write($"{indent}<{element.Name}");
    if (element.Attributes.Any())
    {
        Console.Write($" {string.Join(" ", element.Attributes)}");
    }
    Console.WriteLine($"{indent}</{element.Name}>");
}

return;

async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}

static HtmlElement ProcessHtmlLine(List<string> htmlLines)
{
    var root = new HtmlElement();
    var currentElement = root;
    foreach (var line in htmlLines)
    {


        string id, cl;
        var tagName = line.Split(' ')[0];
        if (tagName == "/html")
        {
            break;
        }
        else if (tagName.StartsWith("/"))
        {
            if (currentElement.Parent != null)
            {
                currentElement = currentElement.Parent;
            }
        }
        else if (HtmlHelper.instance.ArrA.Contains(tagName))
        {

            HtmlElement newElement = new HtmlElement();
            newElement.Name = tagName;


            var restOfLine = line.Remove(0, tagName.Length);
            var attributes = Regex.Matches(restOfLine, "([a-zA-Z]+)=\\\"([^\\\"]*)\\\"")
                        .Cast<Match>()
                        .Select(m => $"{m.Groups[1].Value}=\"{m.Groups[2].Value}\"")
                        .ToList();
            if (attributes.Any(a => a.StartsWith("class")))
            {
                var classAt = attributes.First(a => a.StartsWith("class"));
                var classes = classAt.Split('=')[1].Trim('"').Split(' ');
                newElement.Classes.AddRange(classes);
            }
            newElement.Attributes.AddRange(attributes);

            var idAt = attributes.FirstOrDefault(a => a.StartsWith("id"));
            if (!string.IsNullOrEmpty(idAt))
            {
                newElement.Id = idAt.Split('=')[1].Trim('"');
            }

            newElement.Parent = currentElement;
            currentElement.Children.Add(newElement);
            if (line.EndsWith("/") || HtmlHelper.instance.ArrB.Contains(tagName))
            {
                currentElement = newElement.Parent;
            }
            else
            {
                currentElement = newElement;
            }

        }

        else
        {
            currentElement.InnerHtml = line;

        }
        
    }
    return root;

}






