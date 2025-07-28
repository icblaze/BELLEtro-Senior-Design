// This Document contains the code for the Valentine mentor
// Effect is to have played cards with Voiced/Tense suit give +3 Mult when scored
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Valentine : Mentor
{
    //  Mentor name and basePrice are preset
    public Valentine(CardEdition edition) : base(MentorName.Valentine, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.PreCard };
        description = "Played cards with <b>Voiced</b>/<b>Tense</b> suit give <color=red>+3 Mult</color> when scored";
    }

    //  Played card will add +3 Mult if Voiced/Tense suit
    public override void UseMentor(PCard card)
    {
        if(card.suit == SuitName.Voiced || card.suit == SuitName.Tense || (!card.isDisabled && card.enhancement == CardEnhancement.WildCard))
        {
            ScoringManager.access().IncrementCurrentMult(3);
            ScoreCoroutine(ScoringManager.access().ScorePopupPCard(card, $"<color=red>+3 Mult</color>"));
        }
    }
}
