// This Document contains the code for the LightSnack mentor
// This Mentor effect is to Retrigger all cards for the next 10 hands.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LightSnack : Mentor
{
    Player player = Player.access();
    int snackMult = 20;
    JokerCardHolder mentorCardHolder;

    //  Mentor name and basePrice are preset
    public LightSnack(CardEdition edition) : base(MentorName.LightSnack, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.Post, UseLocation.PostBlind};
        description = "+20 Mult, -4 Mult at end of round";
    }

    //  Update description with uses left
    public override string GetDescription()
    {
        description = $"+{snackMult} Mult, -4 Mult at end of round";
        return description;
    }

    //  Increment Mult by snackMult
    public override void UseMentor()
    {
        if (ScoringManager.access().GetScoringStatus())
        {
            ScoringManager.access().IncrementCurrentMult(snackMult);
        }
        else
        {
            // Decrement snackMult after each hand played (PostHand)
            snackMult -= 4;

            //  Disappears when 0 or below
            if (snackMult <= 0)
            {
                mentorCardHolder = GameObject.FindFirstObjectByType<JokerCardHolder>();
                mentorCardHolder.RemoveMentor(this);
            }
        }
    }

}
