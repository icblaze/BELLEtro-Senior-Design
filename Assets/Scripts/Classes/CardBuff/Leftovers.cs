/// This Document contains the code for the Leftovers CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Leftovers : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Leftovers() : base(CardBuffName.Leftovers)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Show last used consumable in description (if available)
        Consumable lastUsed = game.previousConsumable;
        description = "Generates the last used consumable except Leftovers";

        if(lastUsed is null)
        {
            description += " (None)";
        }
        else if(lastUsed.type == ConsumableType.Textbook)
        {
            Textbook prevTextbook = (Textbook) lastUsed;
            description += " (" + prevTextbook.name.ToString() + ")";
        }
        else if(lastUsed.type == ConsumableType.CardBuff)
        {
            Textbook prevTextbook = (Textbook)lastUsed;
            description += " (" + prevTextbook.name.ToString() + ")";
        }

        return description;
    }

    //  Generates the last used consumable
    public override void applyCardBuff ()
    {
        Consumable lastUsed = game.previousConsumable;
        if (lastUsed.type == ConsumableType.Textbook)
        {
            Textbook prevTextbook = (Textbook) lastUsed;
            player.consumables.Add(new Textbook(prevTextbook.name));
        }
        else
        {
            CardBuff prevCardBuff = (CardBuff) lastUsed;
            player.consumables.Add(CardBuffFactory(prevCardBuff.name));
        }

        //  After adding copy of previous, remove Leftovers if in slot
        RemoveConsumable();

        //  Set prev used consumable to current consumable (except for Leftovers)
    }
}

