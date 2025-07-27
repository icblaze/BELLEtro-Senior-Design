// This Document contains the code for the ChiliPepper CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class ChiliPepper : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();
    DeleteCard deleteScript;

    //  Construct CardBuff consumable with enum for name
    public ChiliPepper() : base(CardBuffName.ChiliPepper)
    {
        isInstant = false;
        isDisabled = false;
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

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Enhances <color=orange>1</color> selected card to be Glass.";
        return description;
    }

    //  TODO Enhances 1 card to "Glass" Card
    public override void applyCardBuff ()
    {
        //  Change enhancement to Gold, update visual
        Card glassCard = deleteScript.GetSelectedCards()[0].GetComponent<Card>();
        glassCard.pcard.enhancement = CardEnhancement.GlassCard;
        glassCard.AssignPCard(glassCard.pcard);

        //  Apply to it in cardsDrawn so that'll it be in deck
        Deck deck = Deck.access();
        for (int i = 0; i < deck.cardsDrawn.Count; i++)
        {
            if (deck.cardsDrawn[i].cardID == glassCard.pcard.cardID)
            {
                deck.cardsDrawn[i] = glassCard.pcard;
                break;
            }

        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

