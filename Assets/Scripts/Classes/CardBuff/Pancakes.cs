// This Document contains the code for the Almonds CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Pancakes : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Pancakes() : base(CardBuffName.Pancakes)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Generates up to <color=orange>2</color> random Card Buffs.";
        return description;
    }

    //  Generates up to 2 random Card Buffs
    public override void applyCardBuff ()
    {
        GenerateConsumables(ConsumableType.CardBuff);

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

