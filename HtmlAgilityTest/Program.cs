using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
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

            GetCities();
            Console.Read();
        }

        static void GetCities()
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
                wc.Headers.Add("origin", "https://recyclemap.ru");
                wc.Headers.Add("content-length", "7");
                wc.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
                wc.Headers.Add("x-requested-with", "XMLHttpRequest");

                NameValueCollection nameValueCollection = new NameValueCollection();
                nameValueCollection.Add("city", cityId);

                var json = wc.UploadValues(RECYCLEMAP_CITY, "POST", nameValueCollection);

                Console.WriteLine(Encoding.ASCII.GetString(json));
            }

            return null;
        }

    }
}
