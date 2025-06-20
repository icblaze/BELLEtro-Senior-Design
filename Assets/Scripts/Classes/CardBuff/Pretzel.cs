// This Document contains the code for the Pretzel CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

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

        //  Disable if no room
        if(player.consumables.Count >= player.maxConsumables)
        {
            isDisabled = true;
        }
        else
        {
            isDisabled = false;
        }

        description = "Generates Textbook for most played hand";
        description += " (" + mostPlayedHand.ToString() + ")";
        return description;
    }

    //  Generates Textbook for most played Textbook hand
    public override void applyCardBuff ()
    {
        player.consumables.Add(new Textbook(mostPlayedHand));

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

