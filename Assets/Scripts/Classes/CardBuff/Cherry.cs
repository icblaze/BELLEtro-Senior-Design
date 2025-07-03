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
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public Cherry() : base(CardBuffName.Cherry)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Select 2 cards. Coverts the left card into the right card.";
        return description;
    }

    //  Disables if 2 cards are not selected
    public override bool CheckDisabled()
    {
        deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        if (deleteScript == null)
        {
            isDisabled = true;
            return isDisabled;
        }

        deleteScript.GetSelectedCards();

        if (deleteScript.GetSelectedCards().Count == 2)
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

    //  Select 2 cards. Converts the left card into right.
    public override void applyCardBuff ()
    {
        //  Will copy the leftmost selected card into the right one
        Card leftCard = deleteScript.GetSelectedCards()[0].GetComponent<Card>();
        Card rightCard = deleteScript.GetSelectedCards()[1].GetComponent<Card>();

        //  Swap order around if doesn't line up with slot order
        if (leftCard.ParentIndex() > rightCard.ParentIndex())
        {
            Card tempCard = leftCard;
            leftCard = rightCard;
            rightCard = tempCard;
        }

        //  Update left card to be copy of right card, maintain left card's ID
        int leftID = leftCard.pcard.cardID;
        leftCard.pcard = rightCard.pcard;
        leftCard.pcard.cardID = leftID;
        leftCard.AssignPCard(leftCard.pcard);

        //  Apply to it in cardsDrawn so that'll it be in deck
        Deck deck = Deck.access();
        for (int i = 0; i < deck.cardsDrawn.Count; i++)
        {
            if (deck.cardsDrawn[i].cardID == leftID)
            {
                deck.cardsDrawn[i] = leftCard.pcard;
                break;
            }
        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

