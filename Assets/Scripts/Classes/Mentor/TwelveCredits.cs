// This Document contains the code for the TwelveCredits mentor
// Effect is if exactly 4 cards are played, +12 Mult
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
        description = "If exactly 4 cards are played, <color=red>+12 Mult";
    }

    //  If exactly 4 cards are played, +12 Mult
    public override void UseMentor()
    {
        //  When this is run, it does consider every card)
        if(ScoringManager.access().GetScoredPCards().Count == 4)
        {
            ScoringManager.access().IncrementCurrentMult(12);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=red>+12 Mult</color>"));
        }
    }
}
