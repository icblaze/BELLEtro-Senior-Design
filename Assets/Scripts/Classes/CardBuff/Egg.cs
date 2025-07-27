// This Document contains the code for the Egg CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Egg : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Egg() : base(CardBuffName.Egg)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Doubles current money (up to <color=#BB8525FF>$20</color>)";
        return description;
    }

    //  Doubles current money (up to $20)
    public override void applyCardBuff ()
    {
        player.moneyCount += Math.Min(player.moneyCount, 20);
        ShopManager shop = ShopManager.access();
        shop.UpdateMoneyDisplay();

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

