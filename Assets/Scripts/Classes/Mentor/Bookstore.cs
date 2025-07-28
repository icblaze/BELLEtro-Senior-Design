// This Document contains the code for the Bookstore mentor
// Effect is X3 Mult if the current hand played has been played before in this round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bookstore : Mentor
{
    private HashSet<string> handNames = new();

    //  Mentor name and basePrice are preset
    public Bookstore(CardEdition edition) : base(MentorName.Bookstore, edition, 8)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "<b><color=red>X3 Mult</color></b> if the current hand played has been played before in this round";
    }

    // X3 Mult if the current hand played has been played before in this round
    public override void UseMentor()
    {
        string playedHand = ScoringManager.access().GetCurrentHandType();

        //  If played before in round
        if(handNames.Contains(playedHand))
        {
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 3);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>X3 Mult</color></b>"));
        }

        handNames.Add(playedHand);
    }

    public void ResetHashSet()
    {
        handNames.Clear();
    }
}
