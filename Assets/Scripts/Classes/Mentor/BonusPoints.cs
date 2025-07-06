// This Document contains the code for the BonusPoints Mentor.
// Earn an extra $1 for every $5 at end of round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class BonusPoints : Mentor
{
    //  Mentor name and basePrice are preset
    public BonusPoints(CardEdition edition) : base(MentorName.BonusPoints, edition, 6)
    {
        //  Might have to change this buffer location?
        locations = new UseLocation[] { UseLocation.PreShop };
        description = "Earn extra $1 for every $5 at end of round (max $5)";
    }

    //  Earn an extra $1 for every $5 at end of round, maximum $5
    public override void UseMentor()
    {
        int extraCount = Player.access().moneyCount / 5;
        extraCount = Math.Min(extraCount, 5);   //  maximum $5

        EndOfRoundManager.access().IncrementMentorReward(extraCount);
    }
}
