// This Document contains the code for the AreaOfExpertise mentor
// Effect is to When hand played, add the number of times hand type played this run to Mult
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class AreaOfExpertise : Mentor
{
    //  Mentor name and basePrice are preset
    public AreaOfExpertise(CardEdition edition) : base(MentorName.AreaOfExpertise, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "When hand played, add the number of times hand type played this run to Mult";
    }

    //  When hand played, add the number of times hand type played this run to Mult
    public override void UseMentor()
    {
        //  Get number of times played
        string handString = ScoringManager.access().GetCurrentHandType();
        TextbookName textbookName = CardModifier.access().GetTextbookFromString(handString);
        int timesPlayed = Player.access().handTable[textbookName].GetTimesPlayed();

        //  Increment mult
        ScoringManager.access().IncrementCurrentMult(timesPlayed);
        ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=red>+{timesPlayed} Mult</color>"));
    }
}
