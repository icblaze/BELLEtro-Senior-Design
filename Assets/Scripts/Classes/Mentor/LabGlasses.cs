// This Document contains the code for the LabGlasses mentor
// Effect is to give +2 Multiplier for every $5 the player has
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class LabGlasses : Mentor
{
    //  Mentor name and basePrice are preset
    public LabGlasses(CardEdition edition) : base(MentorName.LabGlasses, edition, 7)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "Give +2 Mult for every $5 you have";
    }

    //  Give +2 Mult for every $5 you have
    public override void UseMentor()
    {
        int multipleCount = Player.access().moneyCount / 5;
        int mult = multipleCount * 2;

        //  TODO Assign the mult to the variable in round
        
    }
}
