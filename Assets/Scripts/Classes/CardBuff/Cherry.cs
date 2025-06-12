// This Document contains the code for the Cherry CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Cherry() : base(CardBuffName.Cherry)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Disables if 2 cards are not selected

        description = "Select 2 cards. Coverts the left card into the right card.";
        return description;
    }

    //  TODO Select 2 cards. Converts the left card into right.
    public override void applyCardBuff ()
    {
        //  Will copy the leftmost selected card into the right one

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

