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
        locations = new UseLocation[] { UseLocation.AllCards };
        description = "If exactly 4 cards are played, +12 Chips";
    }

    //  Be able to form Straights and Flushes with 4 cards
    public override void UseMentor()
    {
        //  Temporary effect for the demo
        if(ScoringManager.access().GetScoredPCards().Count == 4)
        {
            ScoringManager.access().IncrementCurrentChips(12);
        }
    }
}
