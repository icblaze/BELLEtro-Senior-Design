// This Document contains the code for the Overachiever mentor
// Effect is to gain X0.2 Mult if round cleared in first played hand
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class Overachiever : Mentor
{
    private float xmult = 1;
    private bool isFirstHand = true;

    //  Mentor name and basePrice are preset
    public Overachiever(CardEdition edition) : base(MentorName.Overachiever, edition, 9)
    {
        locations = new UseLocation[] { UseLocation.Post, UseLocation.PostHand };
        description = "Mentor gains <b><color=red>X0.2 Mult</color></b> if round cleared in first played hand (<b><color=red>X1 Mult</color></b>)";
    }

    //  Update description with xmult
    public override string GetDescription()
    {
        description = $"Mentor gains <b><color=red>X0.2 Mult</color></b> if round cleared in first played hand (<b><color=red>X{xmult} Mult</color></b>)";
        return description;
    }

    //  Apply XMult
    public override void UseMentor()
    {
        if(ScoringManager.access().GetScoringStatus())
        {
            //  Apply XMult
            int newMult = (int) Math.Floor(xmult * ScoringManager.access().GetCurrentMult());
            ScoringManager.access().SetCurrentMult(newMult);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>X{xmult} Mult</color></b>"));
        }
        else if(isFirstHand)
        {
            //  If goal completed in first hand of the round, X0.25 added to xmult
            if(Player.access().chipCount >= ScoringManager.access().GetNeededScore())
            {
                xmult += 0.2f;
                ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>Increase XMult</color></b>"));
            }
            isFirstHand = false;
        }
    }

    public void ResetStatus()
    {
        isFirstHand = true;
    }
}
