// This Document contains the code for the Cheese CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Cheese() : base(CardBuffName.Cheese)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string CheckDescription()
    {
        return description;
    }

    //  TODO Enhances 1 card to "Gold" Card
    public override void applyCardBuff ()
    {

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

