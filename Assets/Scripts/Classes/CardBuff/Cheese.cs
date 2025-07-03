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
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public Cheese() : base(CardBuffName.Cheese)
    {
        isInstant = false;
        isDisabled = true;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Enhances 1 selected card to be Gold.";
        return description;
    }

    //  Disables if 1 card isn't selected
    public override bool CheckDisabled()
    {
        deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        if (deleteScript == null)
        {
            isDisabled = true;
            return isDisabled;
        }

        deleteScript.GetSelectedCards();

        if (deleteScript.GetSelectedCards().Count == 1)
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

    //  Enhances 1 card to "Gold" Card
    public override void applyCardBuff ()
    {
        //  Change enhancement to Gold, update visual
        Card goldCard = deleteScript.GetSelectedCards()[0].GetComponent<Card>();
        goldCard.pcard.enhancement = CardEnhancement.GoldCard;
        goldCard.AssignPCard(goldCard.pcard);

        //  Apply to it in cardsDrawn so that'll it be in deck
        Deck deck = Deck.access();
        for (int i = 0; i < deck.cardsDrawn.Count; i++)
        {
            if(deck.cardsDrawn[i].cardID == goldCard.pcard.cardID)
            {
                deck.cardsDrawn[i] = goldCard.pcard;
                break;
            }
        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

