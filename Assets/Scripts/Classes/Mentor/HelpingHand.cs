// This Document contains the code for the HelpingHand Mentor.
// If 95% of required chips are scored, give the remaining 5%
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Numerics;


public class HelpingHand : Mentor
{
    //  Mentor name and basePrice are preset
    public HelpingHand(CardEdition edition) : base(MentorName.HelpingHand, edition, 5)
    {
        //  Might have to change this buffer location?
        locations = new UseLocation[] { UseLocation.Post };
        description = "If 95% of required chips are scored, give the remaining 5%";
    }

    //  If 95% of required chips are scored, give the remaining 5%
    public override void UseMentor()
    {
        //  We can check after every hand played

        //  Find percentage of scored chips this round that 
        float goal = (float)Game.access().BaseChips / (float)Game.access().GetChipTotal();

        //  If at least 95% and not passed
        if (goal >= 0.95 && goal < 1)
        {
            Game.access().BaseChips = Game.access().GetChipTotal();
        }
    }
}
