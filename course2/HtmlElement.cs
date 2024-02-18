using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using course2;
using System.Xml.Linq;

namespace course2
{
    public class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string InnerHtml { get; set; }
        public HtmlElement Parent { get; set; }
        public List<HtmlElement> Children { get; set; }
        public HtmlElement()
        {
            Children = new List<HtmlElement>();
            Attributes = new List<string>();
            Classes = new List<string>();

        }
        public IEnumerable<HtmlElement> Descendants()
        {
            Queue<HtmlElement> q = new Queue<HtmlElement>();
            q.Enqueue(this);
            while (q.Count > 0)
            {
                HtmlElement el = q.Dequeue();
                yield return el;
                foreach (HtmlElement el2 in el.Children) { q.Enqueue(el2); }
            }
        }
        public IEnumerable<HtmlElement> Ancestors()
        {
            yield return this;
            while (Parent != null)
            {
                yield return Parent;
                Parent = Parent.Parent;
            }
        }
    }
}
public static class HtmlElementExtensions
{
    public static HashSet<HtmlElement> elementsMeetSelectorsCteria(this HtmlElement element, Selector selector)
    {
        HashSet<HtmlElement> result = new HashSet<HtmlElement>();

        Recursion(element, selector, result);
        return result;
    }
   private static void Recursion(HtmlElement element, Selector selector, HashSet<HtmlElement> result)
    {
        if (selector.Child == null)
        {
            result.Add(element);
            return;
        }
        var currentElementDescendants = element.Descendants().Where(descendant => descendant.MatchesSelector(selector));

        foreach (var item in currentElementDescendants)
        {
            Recursion(item, selector.Child, result);
        }
    }
   private static bool MatchesSelector(this HtmlElement element, Selector selector)
    {
        return
     (string.IsNullOrEmpty(selector.TagName) || element.Name == selector.TagName) &&
     (string.IsNullOrEmpty(selector.Id) || element.Id == selector.Id) &&
     (selector.Classes.All(cls => element.Classes.Contains(cls)));
    }
}


