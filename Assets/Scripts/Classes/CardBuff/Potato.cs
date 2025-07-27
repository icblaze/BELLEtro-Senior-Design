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
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public Potato() : base(CardBuffName.Potato)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Add a Funding seal to <color=orange>1</color> selected card.";
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

    //  Add "Funding" seal to 1 card
    public override void applyCardBuff ()
    {
        //  Change seal to Funding, update visual
        Card fundCard = deleteScript.GetSelectedCards()[0].GetComponent<Card>();
        fundCard.pcard.seal = CardSeal.Funding;
        fundCard.AssignPCard(fundCard.pcard);

        //  Apply to it in cardsDrawn so that'll it be in deck
        Deck deck = Deck.access();
        for (int i = 0; i < deck.cardsDrawn.Count; i++)
        {
            if (deck.cardsDrawn[i].cardID == fundCard.pcard.cardID)
            {
                deck.cardsDrawn[i] = fundCard.pcard;
                break;
            }

        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

