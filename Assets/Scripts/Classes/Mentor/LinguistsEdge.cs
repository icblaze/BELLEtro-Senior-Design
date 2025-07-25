﻿// This Document contains the code for the LinguistsEdge Mentor.
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
        description = "X2 Mult if scored hand contains 3 or more cards";
    }

    //  X2 Mult if scored hand contains 3 or more cards
    public override void UseMentor()
    {
        if (ScoringManager.access().GetScoredPCards().Count >= 3)
        {
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 2);
        }
    }
}
