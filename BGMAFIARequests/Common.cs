using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGMAFIARequests
{
    public class Common
    {
        public static readonly HttpClient client = new HttpClient();
        public static string lastPath = "";

        public static async Task Login()
        {
            try
            {
                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("z", "Vro"),
                    new KeyValuePair<string, string>("new_ws", "1"),
                    new KeyValuePair<string, string>("login[usr]", "niko50cent"),
                    new KeyValuePair<string, string>("login[pwd]", "test"),
                    new KeyValuePair<string, string>("login[submit]", "Логин"),
                });


                Uri uri = new Uri("http://bgmafia.com/auth/login");
                HttpResponseMessage response = await client.PostAsync(uri, content);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                File.WriteAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html", responseBody);
                Console.WriteLine(response.StatusCode);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static IEnumerable<Cookie> GetCookies()
        {
            try
            {
                CookieContainer cookies = new CookieContainer();
                HttpClientHandler handler = new HttpClientHandler();
                handler.CookieContainer = cookies;

                HttpClient client = new HttpClient(handler);

                client.DefaultRequestHeaders.Add("User-Agent", "C# App");

                var content = new FormUrlEncodedContent(new[]
{
                    new KeyValuePair<string, string>("z", "Vro"),
                    new KeyValuePair<string, string>("new_ws", "1"),
                    new KeyValuePair<string, string>("login[usr]", "niko50cent"),
                    new KeyValuePair<string, string>("login[pwd]", "test"),
                    new KeyValuePair<string, string>("login[submit]", "Логин"),
                });


                Uri uri = new Uri("http://bgmafia.com/auth/login");
                HttpResponseMessage response = client.PostAsync(uri, content).Result;

                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();

                return responseCookies;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return null;
            }
        }

        public static async Task GetPage(string parameters, bool hasRun, bool showSuccess = true)
        {
            try
            {
                Uri uri = new Uri("http://bgmafia.com/");

                if (!hasRun)
                {
                    client.DefaultRequestHeaders.Clear();
                    string cookies = String.Join("; ", GetCookies().Select(o => o.ToString()));

                    client.DefaultRequestHeaders.Add("Cookie", cookies);
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
                }

                HttpResponseMessage response = await client.GetAsync(uri + parameters);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                File.WriteAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html", responseBody);

                if (showSuccess)
                    Console.WriteLine(response.StatusCode);

                lastPath = parameters;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static bool CheckIfThereIsCapcha()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                bool capcha = doc.DocumentNode.InnerHtml.Contains("captcha");

                return capcha;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return false;
            }
        }

        public static bool CheckIfInJail()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                string htmlBody = doc.DocumentNode.InnerHtml;

                if (htmlBody.Contains("minfo"))
                {
                    return true;
                }

                return false;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return false;
            }
        }
    }
}
