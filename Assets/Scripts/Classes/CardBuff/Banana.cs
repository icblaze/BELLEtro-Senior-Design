// This Document contains the code for the Banana CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class Banana : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();

    //  Construct CardBuff consumable with enum for name
    public Banana() : base(CardBuffName.Banana)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        //  If no space available, then disable
        if(player.mentorDeck.Count >= player.maxMentors)
        {
            isDisabled = true;
        }
        else
        {
            isDisabled = false;
        }

        description = "Generate a random Mentor (must have room)";
        return description;
    }

    //  Generate a random Mentor (must have room) 
    public override void applyCardBuff ()
    {
        //  Liable to change depending on random function
        player.mentorDeck.AddRange(game.randomMentorShop(1));

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

