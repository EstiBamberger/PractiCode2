using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using course2;

namespace course2
{
    public class Selector
    {
        public string TagName { get; set; }
        public string Id { get; set; }
        public List<string> Classes { get; set; }
        public Selector Parent { get; set; }
        public Selector Child { get; set; }
        public Selector()
        {
            Classes = new List<string>();

        }
        public static Selector ConvertToSelector(string queryString)
        {
            if (queryString == null)
                return null;

            var list = queryString.Split(' ');
            Selector rootSelector = new Selector();
            Selector currentSelector = rootSelector;
            List<string> parts = new List<string>();
            foreach (var item in list)
            {
                parts = Regex.Matches(item, @"(?:\.|#)?[\w-]+")
                   .Cast<Match>()
                   .Select(m => m.Value)
                   .ToList();
                foreach (var part in parts)
                {
                    if (string.IsNullOrEmpty(part))
                    {
                        continue;
                    }
                    if (part.StartsWith("#"))
                    {
                        currentSelector.Id = part.Substring(1);
                    }
                    else if (part.StartsWith("."))
                    {
                        currentSelector.Classes.Add(part.Substring(1));
                    }
                    else
                    {
                        if (HtmlHelper.instance.ArrA.Contains(part))
                        {
                            currentSelector.TagName = part;
                        }
                    }
                }
                Selector newSelector = new Selector();
                currentSelector.Child = newSelector;
                currentSelector = newSelector;

            }
            return rootSelector;
        }
    }
}




















