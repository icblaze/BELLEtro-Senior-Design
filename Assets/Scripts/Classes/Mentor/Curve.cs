// This Document contains the code for the Curve Mentor.
// Prevents loss if chips scored at least required 25%. Disappears after use.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Numerics;


public class Curve : Mentor
{
    private Round round = Round.access();
    private Player player = Player.access();
    private JokerCardHolder mentorCardHolder;

    //  Mentor name and basePrice are preset
    public Curve(CardEdition edition) : base(MentorName.Curve, edition, 5)
    {
        //  Might have to change this buffer location?
        locations = new UseLocation[] { UseLocation.PostBlind };
        description = "Prevents loss if chips scored at least 25% required. <i>Disappears after use.</i>";
    }

    //  Prevents loss if chips scored at least required 25%. Disappears after use.
    public override void UseMentor()
    {
        //  If final hand 
        if (player.handCount == 0)
        {
            //  Find percentage of scored chips this round relative to target score 
            double goal = (double) player.chipCount / (double) ScoringManager.access().GetNeededScore();

            //  If at least 25% and not passed
            if (goal >= 0.25 && goal < 1)
            {
                //  Let the player win by setting to goal
                ScoringManager.access().SetRoundScore(ScoringManager.access().GetNeededScore());

                //  Remove from player's mentorDeck after effect used
                mentorCardHolder = GameObject.FindFirstObjectByType<JokerCardHolder>();
                mentorCardHolder.RemoveMentor(this);
            }
        }
    }
}
