// This Document contains the code for the BonusPoints Mentor.
// Earn an extra $1 for every $5 at end of round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class BonusPoints : Mentor
{
    //  Mentor name and basePrice are preset
    public BonusPoints(CardEdition edition) : base(MentorName.BonusPoints, edition, 6)
    {
        //  Might have to change this buffer location?
        locations = new UseLocation[] { UseLocation.PostBlind };
        description = "Earn extra $1 for every $5 at end of round";
    }

    //  Earn an extra $1 for every $5 at end of round
    public override void UseMentor()
    {
        int extraCount = Player.access().moneyCount / 5;

        Player.access().moneyCount += extraCount;
    }
}
