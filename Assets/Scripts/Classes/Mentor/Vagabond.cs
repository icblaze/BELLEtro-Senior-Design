// This Document contains the code for the Vagabond mentor
// Spawns a rare Card Buff card when a hand is played, provided players have a free consumable slot and 4 dollars or less when the hand is played.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Vagabond : Mentor
{
    Player player = Player.access();
    Game game = Game.access();
    ConsumableCardHolder consumableCardHolder;

    //  Mentor name and basePrice are preset
    public Vagabond(CardEdition edition) : base(MentorName.Vagabond, edition, 8)
    {
        locations = new UseLocation[] { UseLocation.PostHand };
        description = "Spawns a Card Buff card after a hand is played if player has <color=#BB8525FF>$4</color> or less";
    }

    //  Spawns a Card Buff card after a hand is played if player has $4 or less.
    public override void UseMentor()
    {
        //  If player has $4 or less
        if(player.moneyCount <= 4)
        {
            //  Add random Card Buff to player's consumables slots if not full
            if(player.consumables.Count < player.maxConsumables)
            {
                //  Generate random Card Buff
                CardBuff randCard = game.randomCardBuffShop(1)[0];

                if(consumableCardHolder == null)
                {
                    consumableCardHolder = GameObject.FindAnyObjectByType<ConsumableCardHolder>();
                }
                consumableCardHolder.AddConsumable(randCard);
            }
        }
        
    }
}
