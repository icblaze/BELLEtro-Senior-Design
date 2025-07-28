// This Document contains the code for the HighlyEducated mentor
// Effect is This Mentor gains x0.25 Mult every time a card is added to the deck (X1 Mult)
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class HighlyEducated : Mentor
{
    Deck deckInst = Deck.access();
    private float xmult = 1;

    //  Mentor name and basePrice are preset
    public HighlyEducated(CardEdition edition) : base(MentorName.HighlyEducated, edition, 7)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "This Mentor gains <b><color=red>X0.25 Mult</color></b> every time a card is added to the deck (<b><color=red>X1 Mult</color></b>)";
    }

    //  Calculate multiplier
    public override string GetDescription()
    {
        xmult = 1 + (0.25f * NewCardCount());

        description = $"This Mentor gains <b><color=red>X0.25 Mult</color></b> every time a card is added to the deck (<b><color=red>X{xmult} Mult</color></b>)";
        return description;
    }

    public override void UseMentor()
    {
        xmult = 1 + (0.25f * NewCardCount());
        int newMult = (int) Math.Floor(ScoringManager.access().GetCurrentMult() * xmult);

        ScoringManager.access().SetCurrentMult(newMult);
        ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>X{xmult} Mult</color></b>"));
    }

    //  Check counter and original deck size to see how many cards added
    private float NewCardCount()
    {
        return (float) Deck.counter - Deck.deckSize;
    }
}
