// This Document contains the code for the GradingWeights mentor
// Each scored cards with {suit} suit is retriggered
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GradingWeights : Mentor
{
    private SuitName suit = SuitName.Voiced;

    //  Mentor name and basePrice are preset
    public GradingWeights(CardEdition edition) : base(MentorName.GradingWeights, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.Retrigger };
        description = $"Each scored <b>{suit}</b> card is retriggered (Suit changes each round)";
    }

    public override string GetDescription()
    {
        description = $"Each scored <b>{suit}</b> card is retriggered (Suit changes each round)";
        return description;
    }

    //  Each scored cards with {suit} suit is retriggered
    public override void UseRetriggerMentor(List<PCard> selectedPCards)
    {
        foreach(PCard card in selectedPCards)
        {
            if (card.suit == suit)
            {
                card.replayCounter++;
            }
        }
    }

    //  Change suit after every end of round
    public void RandomizeSuit()
    {
        //  Force it to be different from previous suit
        SuitName newSuit = suit;
        while (newSuit == suit)
        {
            newSuit = (SuitName)UnityEngine.Random.Range(0, Enum.GetNames(typeof(SuitName)).Length);
        }
        suit = newSuit;
    }
}
