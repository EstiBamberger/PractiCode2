using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace course2
{
    internal class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper instance => _instance;
        public string[] ArrA { get; set; }
        public string[] ArrB { get; set; }

        private HtmlHelper()
        {
            var contentA = File.ReadAllText("HtmlTags.json");
            ArrA = JsonSerializer.Deserialize<string[]>(contentA);
            var contentB = File.ReadAllText("HtmlVoidTags.json");
            ArrB = JsonSerializer.Deserialize<string[]>(contentB);
        }
    }
}
