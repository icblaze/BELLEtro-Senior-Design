// This Document contains the code for the Recess mentor
// Effect is that every played card will count in scoring
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class Recess : Mentor
{

    //  Mentor name and basePrice are preset
    public Recess(CardEdition edition) : base(MentorName.Recess, edition, 3)
    {
        locations = new UseLocation[] { };  //  This mentor doesn't have a specific location as effect done in 
        description = "Every played card will count in scoring";
    }

    //  Every played card will count in scoring
    public override void UseMentor()
    {
        //  Check in ScoringManager for logic
    }
}
