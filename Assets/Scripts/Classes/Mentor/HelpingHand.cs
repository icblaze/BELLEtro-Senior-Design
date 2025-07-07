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
        locations = new UseLocation[] { UseLocation.PostHand };
        description = "If 95% of required chips are scored, give the remaining 5%";
    }

    //  If 95% of required chips are scored, give the remaining 5%
    public override void UseMentor()
    {
        //  We can check after every hand played

        //  Find percentage of total chips this round compared to needed score
        double goal = (double)Player.access().chipCount / (double)ScoringManager.access().GetNeededScore();

        //  If at least 95% and not passed
        if (goal >= 0.95 && goal < 1)
        {
            //  Let the player win by setting to goal
            ScoringManager.access().SetRoundScore(ScoringManager.access().GetNeededScore());
        }
    }
}
