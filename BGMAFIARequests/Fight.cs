using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BGMAFIARequests
{
    public class Fight
    {
        public static async Task FightPerson(bool hasRun, bool showSuccess = true)
        {
            try
            {
                if (!hasRun)
                {
                    Common.client.DefaultRequestHeaders.Clear();
                    string cookies = String.Join("; ", Common.GetCookies().Select(o => o.ToString()));

                    Common.client.DefaultRequestHeaders.Add("Cookie", cookies);
                    Common.client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36");
                }

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("z", "uAK"),
                    new KeyValuePair<string, string>("min_level", "37"),
                    new KeyValuePair<string, string>("max_level", "42"),
                    new KeyValuePair<string, string>("max_respect", "55000000"),
                    new KeyValuePair<string, string>("search", "1"),
                });

                HttpResponseMessage response = await Common.client.PostAsync("http://bgmafia.com/matchmaker/find", content);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                File.WriteAllText(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html", responseBody);

                if (showSuccess)
                    Console.WriteLine(response.StatusCode);

                if (Common.CheckIfInJail())
                {
                    Console.WriteLine("You are in jail!");
                }
                else
                {
                    if (Energy.GetEnergy() < 60)
                    {
                        Console.WriteLine("No energy!");
                    }
                    else if (Fightable())
                    {
                        await Attack();
                        Console.WriteLine("Attacked: " + GetOpponentName());
                        if (GetOpponentName() != "No one!")
                        {
                            await Common.GetPage("profile", true, false);
                            Console.WriteLine("Wins: " + GetWinsAndLosses().Key);
                            Console.WriteLine("Losses: " + GetWinsAndLosses().Value);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Not fit!");
                        await FightPerson(true, false);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static bool Fightable()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                int respect = int.Parse(Regex.Replace(doc.DocumentNode.SelectSingleNode("//*[@id=\"cwrapper\"]/div[3]/div[3]/div/div/div/div[2]/table/tbody/tr/td[2]/span[2]").InnerText, @"\s+", String.Empty));

                if (respect < 55000000)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return false;
            }
        }

        public static string GetOpponentName()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                string htmlBody = doc.DocumentNode.InnerHtml;

                if (htmlBody.Contains("-56,11"))
                {
                    return "No one!";
                }

                string name = doc.DocumentNode.SelectSingleNode("//*[@class=\"map_head\"]/tr/td/b/a").InnerText;

                return name;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return null;
            }
        }

        public static async Task Attack()
        {
            try
            {
                HttpResponseMessage response = await Common.client.GetAsync("http://bgmafia.com/" + GetAttackButtonUrl());

                await Common.GetPage(GetAttackButtonUrl(), true);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static string GetAttackButtonUrl()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                string attackButtonUrl = doc.DocumentNode.SelectSingleNode("//*[@id=\"cwrapper\"]/div[3]/div[3]/div/div/div/div[2]/table/tbody/tr/td[3]/a").Attributes["href"].Value.Substring(1);

                return attackButtonUrl;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return null;
            }
        }

        public static KeyValuePair<string, string> GetWinsAndLosses()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                string wins = doc.DocumentNode.SelectSingleNode("//*[@class=\"victory smarttip\"]").InnerText;
                string losses = doc.DocumentNode.SelectSingleNode("//*[@class=\"loss smarttip\"]").InnerText;

                return new KeyValuePair<string, string>(wins, losses);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return new KeyValuePair<string, string>();
            }
        }
    }
}
