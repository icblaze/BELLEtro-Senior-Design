// This Document contains the code for the IceCream CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCream : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();
    DeleteCard deleteScript;
    HorizontalCardHolder horizontalCardHolder;

    //  Construct CardBuff consumable with enum for name
    public IceCream() : base(CardBuffName.IceCream)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Destroys up to <color=orange>2</color> selected cards.";
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

    //  Destroys up to 2 selected cards
    public override void applyCardBuff ()
    {
        if(horizontalCardHolder == null)
        {
            horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        }

        Deck deck = Deck.access();

        //  Destroy cards
        int cardsToRemove = deleteScript.GetSelectedCards().Count;
        for (int i = 0; i < cardsToRemove; i++)
        {
            Card delCard = deleteScript.GetSelectedCards()[0].GetComponent<Card>();
            int delID = delCard.pcard.cardID;   //  Save ID before deleting
            deleteScript.RemoveSelectedCard(deleteScript.GetSelectedCards()[0]);

            Transform parentSlot = delCard.transform.parent;

            GameObject.Destroy(delCard.gameObject);      // Destroy the card
            if (parentSlot != null)
                GameObject.Destroy(parentSlot.gameObject);  // Destroy the parent slot

            horizontalCardHolder.DestroyCardsRefresh();

            //  Remove it in cardsDrawn so that'll it won't be in deck
            for (int j = 0; j < deck.cardsDrawn.Count; j++)
            {
                if (deck.cardsDrawn[j].cardID == delID)
                {
                    deck.cardsDrawn.Remove(deck.cardsDrawn[j]);
                    break;
                }
            }
        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

