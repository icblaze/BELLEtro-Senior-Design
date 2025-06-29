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

    //  Construct CardBuff consumable with enum for name
    public Bagel() : base(CardBuffName.Bagel)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Must have 1 card selected to be enabled

        description = "Add a copy of 1 selected card to your deck";
        return description;
    }

    //  TODO Add a copy of 1 selected card to your deck
    public override void applyCardBuff ()
    {
        //  Find index of PCard selected
        //int index = deck.deckCards.FindIndex();

        //  Add copy to deckCards list
        //deck.deckCards.Add(deck.deckCards[index]);

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

