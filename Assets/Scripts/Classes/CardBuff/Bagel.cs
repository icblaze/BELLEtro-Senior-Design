// This Document contains the code for the Bagel CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Bagel : CardBuff
{
    Game game = Game.access();
    Deck deck = Deck.access();
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public Bagel() : base(CardBuffName.Bagel)
    {
        isInstant = false;
        isDisabled = true;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Add a copy of 1 selected card to your deck";
        return description;
    }

    //  Must have 1 card selected to be enabled
    //  This override sets isDisabled to true if the user has selected 1 card
    public override bool CheckDisabled()
    {
        deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        if (deleteScript == null)
        {
            isDisabled = true;
            return isDisabled;
        }

        if (deleteScript.GetSelectedPCards().Count == 1)
        {
            isDisabled = false;
            return isDisabled;
        }
        else
        {
            isDisabled = true;
            return isDisabled;
        }
    }

    // Add a copy of 1 selected card to your deck
    public override void applyCardBuff ()
    {
        //  Add copy to deckCards list
        deck.AddCard(PCard.CloneCard(deleteScript.GetSelectedPCards()[0]));

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

