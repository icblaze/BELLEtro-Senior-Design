// This Document contains the code for the Potato CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Potato : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Potato() : base(CardBuffName.Potato)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Disable if 1 card isn't selected

        description = "Add a Funding seal to 1 selected card.";
        return description;
    }

    //  TODO Add "Funding" seal to 1 card
    public override void applyCardBuff ()
    {

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

