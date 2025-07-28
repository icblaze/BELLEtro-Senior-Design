// This Document contains the code for the ExtraCredit Mentor.
// Effect is add $1 at end of round.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class ExtraCredit : Mentor
{
    //  Mentor name and basePrice are preset
    public ExtraCredit(CardEdition edition) : base(MentorName.ExtraCredit, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.PreShop };
        description = "Extra <color=#BB8525FF>$1</color> at end of round";
    }

    //  Adds $1 at end of round
    public override void UseMentor()
    {
        EndOfRoundManager.access().IncrementMentorReward(1);
    }
}
