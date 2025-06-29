// This Document contains the code for the FishBait mentor
// +100 chips if played hand contains a straight.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class FishBait : Mentor
{

    //  Mentor name and basePrice are preset
    public FishBait(CardEdition edition) : base(MentorName.FishBait, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "+100 Chips if played hand contains a straight";
    }

    // +100 chips if played hand contains a straight.
    public override void UseMentor()
    {
        //TODO Increase Round Chip variable by 100
        
    }
}
