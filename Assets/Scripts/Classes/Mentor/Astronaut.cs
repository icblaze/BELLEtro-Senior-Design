// This Document contains the code for the Astronaut mentor
// All Textbook cards and Textbook packs in the shop are free.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Astronaut : Mentor
{
    private ShopManager shop;

    //  Mentor name and basePrice are preset
    public Astronaut(CardEdition edition) : base(MentorName.Astronaut, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Shop };
        description = "All Textbook cards and Textbook packs in the shop are free";
    }

    //  All Textbook cards and Textbook packs in the shop are free.
    public override void UseMentor()
    {
        // In shop screen, set price of packs or textbooks to $0
        shop = Object.FindFirstObjectByType<ShopManager>();
        shop.mentorShopEffect(this);
    }
}
