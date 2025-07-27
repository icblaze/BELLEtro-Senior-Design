// This Document contains the code for the Toast CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Toast : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public Toast() : base(CardBuffName.Toast)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Increase <color=blue>Chips</color> by 10 for up to <color=orange>3</color> selected cards";
        return description;
    }

    //  Checks if at least 1 card is selected, at max 3
    public override bool CheckDisabled()
    {
        deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        if (deleteScript == null)
        {
            isDisabled = true;
            return isDisabled;
        }

        deleteScript.GetSelectedCards();

        if (deleteScript.GetSelectedCards().Count >= 1 && deleteScript.GetSelectedCards().Count <= 3)
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

    //  Increase chips by 10 for up to 3 selected cards
    public override void applyCardBuff()
    {
        Deck deck = Deck.access();

        //  Raises +Mult value by 1 for up to 3 selected cards

        for (int i = 0; i < deleteScript.GetSelectedCards().Count; i++)
        {
            //  We should use AssignPCard here 
            Card chipCard = deleteScript.GetSelectedCards()[i].GetComponent<Card>();
            chipCard.pcard.chips += 10;
            chipCard.AssignPCard(chipCard.pcard);

            //  Apply to it in cardsDrawn so that'll it be in deck
            for (int j = 0; j < deck.cardsDrawn.Count; j++)
            {
                if (deck.cardsDrawn[j].cardID == chipCard.pcard.cardID)
                {
                    deck.cardsDrawn[j] = chipCard.pcard;
                    break;
                }
            }
        }


        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

