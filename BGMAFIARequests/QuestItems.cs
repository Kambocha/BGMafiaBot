using System;
using System.Collections.Generic;
using System.Text;

namespace BGMAFIARequests
{
    public class QuestItems
    {
        public Dictionary<string, string> Items { get; } = new Dictionary<string, string>()
        {
          { "Swat"  ,"swat.attack" },
          { "Musicians"  ,"musicians.attack" },
          { "Dealers"  ,"dealers.attack" },
          { "Motorbike-race"  ,"motorbike-race.attack" },
          { "motorbike-rider"  ,"motorbike-rider.attack" },
          { "Dog-walk"  ,"dog-walk.attack" },
          { "Dog-fight"  ,"dog-fight.attack" },
          { "Firefight"  ,"firefight.attack" },
          { "Boss-suv"  ,"boss-suv.attack" },
          { "Interview"  ,"interview.attack" },
          { "Kidnapper"  ,"kidnapper.attack" },
          { "Car-race"  ,"car-race.attack" },
          { "Carnival"  ,"carnival.attack" },
          { "Errand-boy"  ,"errand-boy.attack" },
          { "Bank-robbery"  ,"bank-robbery.attack" },
          { "Bargain"  ,"bargain.attack" },
          { "Limo"  ,"limo.attack" },
          { "Armoured-car", "armoured-car.attack"},
          { "K9 police", "k9.attack"},
          { "Playboy", "playboy.attack"},
          { "Pimp", "pimp.attack"},
          { "Arrest", "arrested.attack"}
        };
    }
}
