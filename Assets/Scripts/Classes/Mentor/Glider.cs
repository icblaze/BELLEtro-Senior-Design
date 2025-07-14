// This Document contains the code for the Glide mentor
// Effect is Gives X0.4 Mult for each glide in your full deck (Currently x1.2)
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class Glider : Mentor
{
    Deck deckInst = Deck.access();
    float glideMult = 0.4f;

    //  Mentor name and basePrice are preset
    public Glider(CardEdition edition) : base(MentorName.Glider, edition, 7)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "Gives X0.4 Mult for each glide in your full deck (Currently x1.2)";
    }

    //  Calculate multiplier
    public override string GetDescription()
    {
        glideMult = 0.4f * GlideCount();

        description = $"Gives X0.4 Mult for each glide in your full deck (Currently x{glideMult})";
        return description;
    }

    public override void UseMentor()
    {
        glideMult = 0.4f * GlideCount();
        int newMult = (int) Math.Ceiling(ScoringManager.access().GetCurrentMult() * glideMult);

        ScoringManager.access().SetCurrentMult(newMult);
    }

    //  Check both piles for the amount of glides
    private float GlideCount()
    {
        float glideNum = 0;

        foreach (PCard card in deckInst.deckCards)
        {
            if(IsGlide(card))
            {
                glideNum++;
            }
        }

        foreach (PCard card in deckInst.cardsDrawn)
        {
            if (IsGlide(card))
            {
                glideNum++;
            }
        }

        return glideNum;
    }

    private bool IsGlide(PCard card)
    {
        return card.term == LinguisticTerms.Voiced_Glide_AlveoPalatal || card.term == LinguisticTerms.Voiced_Glide_Velar || card.term == LinguisticTerms.Voiceless_Glide_Velar;
    }
}
