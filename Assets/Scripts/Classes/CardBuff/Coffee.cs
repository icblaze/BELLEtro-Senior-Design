/// This Document contains the code for the Coffee CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Coffee : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Coffee() : base(CardBuffName.Coffee)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string CheckDescription()
    {
        return description;
    }

    //  TODO Add "Retake" seal to 1 card
    public override void applyCardBuff ()
    {
        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

