// This Document contains the code for the Triplets mentor
// +12 Mult if played hand contains three of a kind
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Triplets : Mentor
{

    //  Mentor name and basePrice are preset
    public Triplets(CardEdition edition) : base(MentorName.Triplets, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "+12 Mult if played hand contains three of a kind";
    }

    // +12 Mult if played hand contains three of a kind
    public override void UseMentor()
    {
        //TODO Add +12 Mult to multiplier

    }
}
