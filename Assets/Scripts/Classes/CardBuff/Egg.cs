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
    public override string CheckDescription()
    {
        return description;
    }

    //  Doubles current money (up to $20)
    public override void applyCardBuff ()
    {
        player.moneyCount += Math.Min(player.moneyCount * 2, 20);

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

