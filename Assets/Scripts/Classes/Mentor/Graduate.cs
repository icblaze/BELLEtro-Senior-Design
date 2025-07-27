// This Document contains the code for the Graduate mentor
// Effect is Diphthongs Played get plus 20 chips and 4 Mult when scored.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Graduate : Mentor
{
    //  Mentor name and basePrice are preset
    public Graduate(CardEdition edition) : base(MentorName.Graduate, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.PreCard };
        description = "<color=blue>+20 Chips</color> and <color=red>+4 Mult</color> when Dipthongs are scored";
    }

    public override void UseMentor(PCard pcard)
    {
        //  Check if PCard is Dipthong
        if(pcard.isDiphthong)
        {
            ScoringManager.access().IncrementCurrentChips(20);
            ScoringManager.access().IncrementCurrentMult(4);
        }
    }
}
