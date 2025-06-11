// This Document contains the code for the Milk CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Milk : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Milk() : base(CardBuffName.Milk)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string CheckDescription()
    {
        return description;
    }

    //  Gives total sell value of current mentors
    public override void applyCardBuff ()
    {
        int totalSell = 0;
        foreach (Mentor mentor in player.mentorDeck)
        {
            totalSell += mentor.sellValue;
        }
        player.moneyCount += totalSell;

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

