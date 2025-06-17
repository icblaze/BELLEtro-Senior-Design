// This Document contains the code for the TwelveCredits mentor
// Effect is to be able to form Straight or Flush with 4 cards
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class TwelveCredits : Mentor
{
    //  Mentor name and basePrice are preset
    public TwelveCredits(CardEdition edition) : base(MentorName.TwelveCredits, edition, 7)
    {
        locations = new UseLocation[] { UseLocation.Initial };
        description = "Be able to form Straights and Flushes with 4 cards";
    }

    //  Be able to form Straights and Flushes with 4 cards
    public override void UseMentor()
    {
        //  TODO Set a flag in the hand check that will disable needing a 5th card for both the Straight and Flush hands  
    }
}
