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
        const string RECYCLEMAP_CITY = "https://recyclemap.ru/index.php?option=com_greenmarkers&task=get_json&type=points&tmpl=component";

        private static WebProxy wp;

        static void Main(string[] args)
        {
            wp = new WebProxy("10.214.104.214", 3128);
            wp.UseDefaultCredentials = true;
            //Console.WriteLine(LoadMainPage());
            testc();
            Console.Read();
        }

        static void testc()
        {
            using(WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                wc.Proxy = wp;
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
                wc.Proxy = wp;
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded; charset=UTF-8";
                wc.Headers[HttpRequestHeader.Cookie] = "_gcl_au=1.1.1726520692.1570016363; _ym_uid=1570016363759045945; _ym_d=1570016363; _ga=GA1.2.586811617.1570016364; 469838ec60a6378c83773a23bb93b954=a88643ff103a75776074ce554acdc929; _ym_isad=1; _gid=GA1.2.965543693.1570163046; _dc_gtm_UA-75857896-1=1";
                wc.Headers[HttpRequestHeader.Referer] = "https://recyclemap.ru";
                //wc.Headers[HttpRequestHeader.] = "application/x-www-form-urlencoded";
                //            city: 28
                //layer: 0
                //gos:
                var vm = new { city = cityId};

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
