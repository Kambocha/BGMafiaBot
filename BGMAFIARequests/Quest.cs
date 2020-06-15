using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGMAFIARequests
{
    public class Quest
    {
        public static async Task FindQuest()
        {
            try
            {
                string questLocation = GetQuestLocationLink().Substring(4);

                string coords = questLocation.Remove(questLocation.IndexOf("?"));

                await Common.GetPage(GetQuestLocationLink(), true, false);

                if (!string.IsNullOrEmpty(GetQuestLocationLink()))
                {
                    Console.WriteLine("Moved to: " + GetQuestLocationLink());
                }
                else
                {
                    await ClickQuestItem(coords);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static string GetQuestLocationLink()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

            string htmlBody = doc.DocumentNode.InnerHtml;

            if (htmlBody.Contains("id='blinker'"))
            {
                string arrowUrl = doc.DocumentNode.SelectSingleNode("//*[@id=\"blinker\"]").Attributes["href"].Value.Substring(1);

                return arrowUrl;
            }
            else
            {
                return null;
            }
        }

        public static async Task ClickQuestItem(string coords)
        {
            try
            {
                if (string.IsNullOrEmpty(GetQuestItemLink(coords)))
                {
                    if (Common.CheckIfThereIsCapcha())
                    {
                        Console.WriteLine("CAPTCHA");
                    }
                    else
                    {
                        Console.WriteLine("No quest!");
                    }
                }
                else
                {
                    Console.WriteLine("Clicked: " + GetQuestItemLink(coords));
                }

                await Common.GetPage(GetQuestItemLink(coords), true, false);

                if (Energy.GetEnergy() < 40)
                {
                    Console.WriteLine("No energy!");
                }
                else
                {
                    while (!string.IsNullOrEmpty(GetQuestItemLink(coords)))
                    {
                        await ClickQuestItem(coords);
                    }
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static string GetQuestItemLink(string coords)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

            var htmlBodyString = doc.DocumentNode.InnerHtml;

            QuestItems questItems = new QuestItems();

            foreach (var item in questItems.Items.Values)
            {
                if (htmlBodyString.Contains(item))
                {
                    string questItemUrl = "map/" + coords + "/" + item + "?" + GetRequestId();

                    //-48~15/motorbike-race.attack?z=R1N
                    return questItemUrl;
                }
            }

            return null;
        }

        public static string GetRequestId()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

            var htmlBodyString = doc.DocumentNode.InnerHtml;

            if (htmlBodyString.Contains("REQ_PAIR"))
            {
                string id = htmlBodyString.Substring(htmlBodyString.IndexOf("REQ_PAIR") + 12).Remove(5);
                return id;
            }

            return null;
        }

        public static string GetCurrentCoords()
        {
            HtmlDocument doc = new HtmlDocument();
            doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");
            string minimapUrl = "";

            try
            {
                minimapUrl = doc.DocumentNode.SelectSingleNode("//*[@class=\"go-minimap\"]").Attributes["href"].Value.Substring(10);
            }
            catch (Exception e)
            {
                Console.WriteLine("No quest item in page!", e.Message);
            }

            string coords = minimapUrl.Remove(minimapUrl.IndexOf("?"));

            return coords;
        }
    }
}
