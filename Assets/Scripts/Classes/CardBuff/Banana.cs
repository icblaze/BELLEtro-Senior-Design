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
    JokerCardHolder mentorHolder;

    //  Construct CardBuff consumable with enum for name
    public Banana() : base(CardBuffName.Banana)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "Generate a random Mentor (must have room)";
        return description;
    }

    //  Check Mentor capacity
    public override bool CheckDisabled()
    {
        //  Accounting for negatives, check if there is room for a mentor
        int nonNegativeCount = 0;
        foreach (Mentor mentor in player.mentorDeck)
        {
            if(mentor.edition != CardEdition.Negative)
            {
                nonNegativeCount++;
            }
        }

        if(nonNegativeCount >= player.maxMentors)
        {
            isDisabled = true;
            return isDisabled;
        }
        else
        {
            isDisabled = false;
            return isDisabled;
        }
    }

    //  Generate a random Mentor (must have room) 
    public override void applyCardBuff ()
    {
        if(mentorHolder == null)
        {
            mentorHolder = GameObject.FindFirstObjectByType<JokerCardHolder>();
        }

        //  Generate random mentor that is base edition
        Mentor newMentor = game.randomMentorShop(1)[0];
        newMentor.edition = CardEdition.Base;   
        mentorHolder.AddMentor(newMentor);

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

