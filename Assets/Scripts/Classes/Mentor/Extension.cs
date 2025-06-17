// This Document contains the code for the Extension mentor
// Effect is to have the next purchased Mentor is $3 off
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Extension : Mentor
{
    //  Mentor name and basePrice are preset
    public Extension(CardEdition edition) : base(MentorName.Extension, edition, 7)
    {
        locations = new UseLocation[] { UseLocation.Shop };
        description = "Next purchased Mentor is $3 off";
    }

    //  Next purchased Mentor is $3 off
    public override void UseMentor()
    {
        // TODO Be able to access ShopManager's variables

        // Set the next available Mentor to be $3 off once shop screen is reached
    }
}
