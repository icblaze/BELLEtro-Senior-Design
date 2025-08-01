﻿// This Document contains the code for the WildCard mentor
// Effect is +2 Mult for every unique term held in hand
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class WildCard : Mentor
{
    PCardIDComparer pcardComparer = new PCardIDComparer();

    //  Mentor name and basePrice are preset
    public WildCard(CardEdition edition) : base(MentorName.WildCard, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.PreFromDraw };
        description = "<color=red>+2 Mult</color> for every unique term held in hand";
    }

    // +2 Mult for every unique term held in hand
    public override void UseMentor(PCard card)
    {
        int occurenceCounter = 0;

        foreach(PCard heldCard in Deck.access().heldHand)
        {
            if(card.term == heldCard.term)
            {
                occurenceCounter++;
            }
        }

        //  If only appears once
        if(occurenceCounter == 1)
        {
            ScoringManager.access().IncrementCurrentMult(2);
            ScoreCoroutine(ScoringManager.access().ScorePopupHeld(card, $"<color=red>+2 Mult</color>"));
        }
    }
}
