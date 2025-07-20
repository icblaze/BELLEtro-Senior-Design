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
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public Flatbread() : base(CardBuffName.Flatbread)
    {
        isInstant = false;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Raise +1 Mult for up to 3 selected cards";
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

    //  Raises +Mult value by 1 for up to 3 selected cards
    public override void applyCardBuff ()
    {
        Deck deck = Deck.access();

        //  Raises +Mult value by 1 for up to 3 selected cards

        for (int i = 0; i < deleteScript.GetSelectedCards().Count; i++)
        {
            //  We should AssignPCard here
            Card multCard = deleteScript.GetSelectedCards()[i].GetComponent<Card>();
            multCard.pcard.multiplier += 1;
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

