using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BGMAFIARequests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Login - 1");
            //Console.WriteLine("Get Page - 2");
            //Console.WriteLine("Get Cookies - 3");
            Console.WriteLine("Get Energy - 4");
            Console.WriteLine("Fill Energy - 5");
            Console.WriteLine("Do Quest - 6");
            Console.WriteLine("Search For Fight - 7");
            while(true)
            {
                int input = int.Parse(Console.ReadLine());

                switch (input)
                {
                    case 1:
                        await Common.Login();
                        break;
                    case 2:
                        Console.WriteLine("Add path:");
                        string path = Console.ReadLine();
                        await Common.GetPage(path, false);
                        break;
                    case 3:
                        Common.GetCookies();
                        break;
                    case 4:
                        await Common.GetPage("bar//36907", false);
                        Console.WriteLine("Energy: " + Energy.GetEnergy());
                        break;
                    case 5:
                        await Common.GetPage("bar//36907", false);
                        if (Common.CheckIfInJail())
                        {
                            Console.WriteLine("You are in jail!");
                        }
                        else
                        {
                            while (Energy.GetEnergy() < 180)
                                await Energy.FillEnergy();
                            if (Energy.GetEnergy() == 180)
                                Console.WriteLine("Energy full!");
                        }
                        break;
                    case 6:
                        await Common.Login();
                        if (Common.CheckIfInJail())
                        {
                            Console.WriteLine("You are in jail!");
                        }
                        else
                        {
                            if (Common.CheckIfThereIsCapcha())
                            {
                                Console.WriteLine("CAPTCHA");
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(Quest.GetQuestLocationLink()))
                                {
                                    await Quest.ClickQuestItem(Quest.GetCurrentCoords());
                                }
                                while (!string.IsNullOrEmpty(Quest.GetQuestLocationLink()))
                                {
                                    await Quest.FindQuest();
                                }
                            }
                        }
                        break;
                    case 7:
                        await Fight.FightPerson(false, false);
                        break;
                    default:
                        break;
                }
            }

        }

    }
}
