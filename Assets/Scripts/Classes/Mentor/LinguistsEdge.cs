// This Document contains the code for the LinguistsEdge Mentor.
// Effect is X2 Mult if scored hand contains 3 or more cards
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Numerics;


public class LinguistsEdge : Mentor
{
    //  Mentor name and basePrice are preset
    public LinguistsEdge(CardEdition edition) : base(MentorName.LinguistsEdge, edition, 9)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "<b><color=red>X2 Mult</color></b> if scored hand contains 3 or more cards";
    }

    //  X2 Mult if scored hand contains 3 or more cards
    public override void UseMentor()
    {
        if (ScoringManager.access().GetScoredPCards().Count >= 3)
        {
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 2);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>X2 Mult</color></b>"));
        }
    }
}
