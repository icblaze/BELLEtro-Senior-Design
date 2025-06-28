// This Document contains the code for the LibraryCard mentor
// Effect is to give $1 at end of round per unique Textbook used
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class LibraryCard : Mentor
{
    //  Mentor name and basePrice are preset
    public LibraryCard(CardEdition edition) : base(MentorName.LibraryCard, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.PostBlind };
        description = "Give $1 at end of round per unique Textbook used";
    }

    //  Give $1 at end of round per unique Textbook used
    public override void UseMentor()
    {
        Player player = Player.access();
        int uniqueCount = 0;

        //  Iterate through player's handTable
        for(int tbook = 0; tbook < player.handTable.Count; tbook++)
        {
            //  If a hand's level is above 1, then textbook has been used for it
            if(player.handTable[(TextbookName)tbook].level > 1)
            {
                uniqueCount++;
            }
        }

        //  Add to player's moneyCount
        player.moneyCount += uniqueCount;
    }
}
