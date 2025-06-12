// This Document contains the code for the Almonds CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Cookies : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Cookies() : base(CardBuffName.Cookies)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Checks if at least 1 card selected, max 2

        description = "Enhances up to 2 selected cards to Mult cards";
        return description;
    }

    //  TODO Enhances up to 2 selected cards to "Mult" cards
    public override void applyCardBuff ()
    {

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

