using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BGMAFIARequests
{
    public class Energy
    {
        #region Energy

        public static async Task FillEnergy()
        {
            try
            {
                HttpResponseMessage response = await Common.client.GetAsync("http://bgmafia.com/" + GetFirstDrinkUrl());

                await Common.GetPage(GetFirstDrinkUrl(), true, false);
                Console.WriteLine("Energy: " + GetEnergy());
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
            }
        }

        public static int GetEnergy()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                int energy = int.Parse(doc.DocumentNode.SelectSingleNode("//*[@class=\"u-property\"]/a/u").InnerText);

                return energy;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return 0;
            }
        }

        public static string GetFirstDrinkUrl()
        {
            try
            {
                HtmlDocument doc = new HtmlDocument();
                doc.Load(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "/response.html");

                string drinkUrl = doc.DocumentNode.SelectSingleNode("//*[@class=\"item\"]/a").Attributes["href"].Value.Substring(1);

                return drinkUrl;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("\nExceptionCaught!");
                Console.WriteLine("Message: {0}", e.Message);
                return null;
            }
        }

        #endregion
    }
}
