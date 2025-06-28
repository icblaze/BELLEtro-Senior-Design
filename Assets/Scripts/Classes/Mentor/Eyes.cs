// This Document contains the code for the Eyes mentor
// Effect is Plus 8 Mult if played hand contains any kind of pair.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Eyes : Mentor
{
    Round round = Round.access();

    //  Mentor name and basePrice are preset
    public Eyes(CardEdition edition) : base(MentorName.Eyes, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "+8 Mult if played hand contains a Pair";
    }

    //  +8 Mult if played hand contains a Pair
    public override void UseMentor()
    {
        // Add +8 Mult to multiplier
    }
}
