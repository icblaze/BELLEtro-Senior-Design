// This Document contains the code for the FrequencyHopper mentor
// This Mentor gains +1 Mult if hand contains vowel card
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class FrequencyHopper : Mentor
{
    int mult = 0;

    //  Mentor name and basePrice are preset
    public FrequencyHopper(CardEdition edition) : base(MentorName.FrequencyHopper, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Initial, UseLocation.Post };
        description = "This Mentor gains <color=red>+1 Mult</color> if scored hand contains a vowel (<color=red>+0 Mult</color>)";
    }

    //  Update description with mult variable
    public override string GetDescription()
    {
        description = $"This Mentor gains <color=red>+1 Mult</color> if scored hand contains a vowel (<color=red>+{mult} Mult</color>)";
        return description;
    }

    //  Add mentor's mult to round mult variable 
    public override void UseMentor()
    {
        if (ScoringManager.access().GetScoringStatus())
        {
            ScoringManager.access().IncrementCurrentMult(mult);    //  If scoring, increment mult
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=red>+{mult} Mult</color>"));
        }
        else
        {
            //  Check if hand has vowel
            foreach(PCard card in ScoringManager.access().GetScoredPCards())
            {
                //  Is term a vowel
                if(card.term >= LinguisticTerms.Tense_High_Front && card.term <= LinguisticTerms.Lax_Low_Front)
                {
                    mult++;
                    ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=red>+1 Mult</color>"));
                    return;
                }
            }
        }
    }
}
