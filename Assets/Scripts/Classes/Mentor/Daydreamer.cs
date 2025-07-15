// This Document contains the code for the Daydreamer mentor
// Effect is 50% chance for Mentor to gain +4 Mult at the start of a round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class Daydreamer : Mentor
{
    private int mult = 0;

    //  Mentor name and basePrice are preset
    public Daydreamer(CardEdition edition) : base(MentorName.Daydreamer, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Blind, UseLocation.Post };
        description = "50% chance for Mentor to gain +4 Mult at the start of a round (+0 Mult)";
    }

    //  Update description with +mult
    public override string GetDescription()
    {
        description = $"50% chance for Mentor to gain +4 Mult at the start of a round (+{mult} Mult)";
        return description;
    }

    //  Apply +mult and roll for increment at beginning of round
    public override void UseMentor()
    {
        if(ScoringManager.access().GetScoringStatus())
        {
            ScoringManager.access().IncrementCurrentMult(mult);
        }
        else
        {
            //  When blind selected, roll for chance
            int chance = UnityEngine.Random.Range(0, 2);

            //  Successful roll
            if(chance == 1)
            {
                mult += 4;
            }
        }
    }

}
