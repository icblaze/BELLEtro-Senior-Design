// This Document contains the code for the Flatbread CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Flatbread : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Flatbread() : base(CardBuffName.Flatbread)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Check if at least 1 card is selected, max 3

        description = "Raise +1 Mult for up to 3 selected cards";
        return description;
    }

    //  TODO Converts up to 3 selected cards into random Voiceless cards
    public override void applyCardBuff ()
    {

        //  Raises +Mult value by 1 for up to 3 selected cards

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

