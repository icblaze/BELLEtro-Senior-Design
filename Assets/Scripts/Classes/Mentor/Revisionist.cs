// This Document contains the code for the Revisionist mentor
// Effect is to reroll once for free after each Blind
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Revisionist : Mentor
{
    public bool hasRerolled = false;

    //  Mentor name and basePrice are preset
    public Revisionist(CardEdition edition) : base(MentorName.Revisionist, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Shop };
        description = "Reroll once for free after each Blind";
    }

    public override void UseMentor()
    {
        if (!hasRerolled)
        {
            ShopManager.access().mentorShopEffect(this);
            hasRerolled = true;
        }
    }
}
