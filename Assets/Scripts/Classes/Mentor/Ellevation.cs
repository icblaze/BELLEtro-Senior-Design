// This Document contains the code for the Ellevation mentor
// This Mentor gains +8 Chips when each played /ɛ/ or /l/ is scored.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Ellevation : Mentor
{
    int chips = 0;

    //  Mentor name and basePrice are preset
    public Ellevation(CardEdition edition) : base(MentorName.ELLEvation, edition, 8)
    {
        locations = new UseLocation[] { UseLocation.PreCard, UseLocation.Post };
        description = "This Mentor gains <color=blue>+8 Chips</color> when each played /ɛ/ or /l/ is scored (<color=blue>+0 Chips</color>)";
    }

    //  Update description with chips variable
    public override string GetDescription()
    {
        description = "This Mentor gains <color=blue>+8 Chips</color> when each played /ɛ/ or /l/ is scored  (+<color=blue>" + chips + " Chips</color>)";
        return description;
    }

    //  Will add +8 chips when each played /ɛ/ or /l/ is scored (Pre)
    public override void UseMentor(PCard pcard)
    {
        // /ɛ/ or /l/
        if (pcard.term == LinguisticTerms.Lax_Mid_Front || pcard.term == LinguisticTerms.Voiced_Liquid_Alveolar_Lateral)
        {
            chips += 8;
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=blue>+8 Chips</color>"));
        }
    }

    //  Add mentor's chips to round chip variable (Post)
    public override void UseMentor()
    {
        ScoringManager.access().IncrementCurrentChips(chips);
        ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=blue>+{chips} Chips</color>"));
    }
}
