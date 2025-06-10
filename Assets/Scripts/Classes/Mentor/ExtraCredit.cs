// This Document contains the code for the CheatSheet Mentor.
// Effect is to copy the effect of the Mentor to its right.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class ExtraCredit : Mentor
{
    //  Mentor name and basePrice are preset
    public ExtraCredit(CardEdition edition) : base(MentorName.ExtraCredit, edition, 5)
    {
        //  Might have to change this buffer location?
        locations = new UseLocation[] { UseLocation.Shop };
        description = "Earn $1 at end of round";
    }

    //  Adds $1 at end of round
    public override void UseMentor()
    {
        Player.access().moneyCount++;
    }
}
