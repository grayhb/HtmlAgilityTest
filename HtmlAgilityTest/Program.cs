using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace HtmlAgilityTest
{
    class Program
    {
        const string RECYCLEMAP = "https://recyclemap.ru/";
        const string RECYCLEMAP_CITIES = "https://recyclemap.ru/index.php?task=get_json&type=cities&tmpl=component";
        const string RECYCLEMAP_CITY = "https://recyclemap.ru/index.php?&task=get_json&type=points&tmpl=component";

        static void Main(string[] args)
        {
            //Console.WriteLine(LoadMainPage());
            testc();
            Console.Read();
        }

        static void testc()
        {
            using(WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";

                var json = wc.DownloadString(RECYCLEMAP_CITIES);
                var obj = JArray.Parse(json);

                dynamic city = obj[2];
                Console.WriteLine(city.title);
                Console.WriteLine(city.id);
                GetPoints(city.id.ToString());
            }
        }

        static JArray GetPoints(string cityId)
        {
            //TODO: POST QUERY - FORM DATA - city:1
            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                wc.Headers[HttpRequestHeader.] = "application/x-www-form-urlencoded";
                //            city: 28
                //layer: 0
                //gos:
                var vm = new { city = cityId, layer = 0, gos = 0 };

                var json = wc.UploadString(RECYCLEMAP_CITY, "POST", JsonConvert.SerializeObject(vm));
                Console.WriteLine(json);
            }

            return null;
        }

        //static string LoadMainPage()
        //{
        //    var web = new HtmlWeb();
        //    var doc = web.Load(RECYCLEMAP);

        //    List<int> cityIds = new List<int>();

        //    foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//select[@id='city']//option"))
        //    {
        //        //Console.WriteLine("Value=" + node.Attributes["value"].Value);
        //        //Console.WriteLine("InnerText=" + node.InnerText);
        //        //Console.WriteLine();

        //        if (int.TryParse(node.Attributes["value"].Value, out int cityId))
        //            cityIds.Add(cityId);
        //    }

        //    return "";
        //}
    }
}
