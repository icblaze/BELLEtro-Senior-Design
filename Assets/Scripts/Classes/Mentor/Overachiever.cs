﻿// This Document contains the code for the Overachiever mentor
// Effect is to gain X0.25 Mult if round cleared in first played hand
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
        description = "Mentor gains X0.25 Mult if round cleared in first played hand (Currently X1 Mult)";
    }

    //  Update description with xmult
    public override string GetDescription()
    {
        description = $"Mentor gains X0.25 Mult if round cleared in first played hand (Currently X{xmult} Mult)";
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
        }
        else if(isFirstHand)
        {
            //  If goal completed in first hand of the round, X0.25 added to xmult
            if(Player.access().chipCount >= ScoringManager.access().GetNeededScore())
            {
                xmult += 0.25f;
            }
            isFirstHand = false;
        }
    }

    public void ResetStatus()
    {
        isFirstHand = true;
    }
}
