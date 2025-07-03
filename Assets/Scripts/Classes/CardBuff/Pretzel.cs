// This Document contains the code for the Pretzel CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;
using SplitString;

public class Pretzel : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();
    TextbookName mostPlayedHand = TextbookName.HighCard;

    //  Construct CardBuff consumable with enum for name
    public Pretzel() : base(CardBuffName.Pretzel)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  Find most used hand (highest rank breaks tie)
        int max = 0;
        mostPlayedHand = TextbookName.HighCard;

        foreach(TextbookName tbook in player.handTable.Keys)
        {
            int timesPlayed = player.handTable[tbook].GetTimesPlayed();
            if (timesPlayed >= max)
            {
                max = timesPlayed;
                mostPlayedHand = tbook;
            }
        }

        description = "Generates Textbook for most played hand";
        description += " (" + SplitCase.Split(mostPlayedHand.ToString()) + ")";
        return description;
    }

    //  Either have space or be the card in consumable slots be the free space
    public override bool CheckDisabled()
    {
        int space = player.maxConsumables - player.consumables.Count;

        //  Check if in player's consumables, free slot made
        //  Check if in player's consumables, free slot made
        foreach (Consumable consumable in player.consumables)
        {
            if (consumable.type == ConsumableType.CardBuff)
            {
                CardBuff inSlot = (CardBuff)consumable;
                if (inSlot == this)
                {
                    space++;
                }
            }
        }

        if (space > 0)
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

    //  Generates Textbook for most played Textbook hand
    public override void applyCardBuff ()
    {
        if (consumableHolder == null)
        {
            consumableHolder = GameObject.FindFirstObjectByType<ConsumableCardHolder>();
        }

        consumableHolder.AddConsumable(new Textbook(mostPlayedHand));

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

