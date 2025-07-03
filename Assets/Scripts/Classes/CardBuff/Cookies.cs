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
    DeleteCard deleteScript;

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

    //  Checks if at least 1 card is selected, at max 2
    public override bool CheckDisabled()
    {
        deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        if (deleteScript == null)
        {
            isDisabled = true;
            return isDisabled;
        }

        deleteScript.GetSelectedCards();

        if (deleteScript.GetSelectedCards().Count >= 1 && deleteScript.GetSelectedCards().Count <= 2)
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

    //  Enhances up to 2 selected cards to "Mult" cards
    public override void applyCardBuff()
    {
        Deck deck = Deck.access();

        //  Change enhancement to Bonus, update visual

        for (int i = 0; i < deleteScript.GetSelectedCards().Count; i++)
        {
            Card multCard = deleteScript.GetSelectedCards()[i].GetComponent<Card>();
            multCard.pcard.enhancement = CardEnhancement.MultCard;
            multCard.AssignPCard(multCard.pcard);

            //  Apply to it in cardsDrawn so that'll it be in deck

            for (int j = 0; j < deck.cardsDrawn.Count; j++)
            {
                if (deck.cardsDrawn[j].cardID == multCard.pcard.cardID)
                {
                    deck.cardsDrawn[j] = multCard.pcard;
                    break;
                }
            }
        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

