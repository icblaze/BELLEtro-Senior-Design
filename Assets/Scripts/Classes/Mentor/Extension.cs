// This Document contains the code for the Extension mentor
// Effect is to have the next purchased Mentor to be $3 off
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Extension : Mentor
{
    private ShopManager shop;

    //  Flag to only discount once per shop
    public bool hasDiscounted = false;

    //  Mentor name and basePrice are preset
    public Extension(CardEdition edition) : base(MentorName.Extension, edition, 7)
    {
        locations = new UseLocation[] { UseLocation.Shop };
        description = "Initial Mentors in Shop are <color=#BB8525FF>$3</color> off";
    }

    //  Next purchased Mentor is $3 off
    public override void UseMentor()
    {
        // Set the next available Mentor to be $3 off once shop screen is reached
        if(!hasDiscounted)
        {
            ShopManager.access().mentorShopEffect(this);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, "Discounted!"));
            hasDiscounted = true; //    Only activate once per shop
        }
    }
}
