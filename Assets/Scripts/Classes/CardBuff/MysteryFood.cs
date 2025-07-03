// This Document contains the code for the MysteryFood CardBuff subclass
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Andy (flakid): made class 


using System;
using System.Collections.Generic;
using UnityEngine;

public class MysteryFood : CardBuff
{
    Game game = Game.access();
    Player player = Player.access();
    List<int> baseMentors;
    JokerCardHolder mentorHolder;

    //  Construct CardBuff consumable with enum for name
    public MysteryFood() : base(CardBuffName.MysteryFood)
    {
        isInstant = true;
        isDisabled = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public override string GetDescription()
    {
        description = "25% chance to add an Edition to a random Mentor";
        return description;
    }

    //  Get list of Base Mentors' indices in player's mentorDeck
    public override bool CheckDisabled()
    {
        baseMentors = new List<int>();
        baseMentors.Clear();
        int index = 0;
        foreach (Mentor mentor in player.mentorDeck)
        {
            if (mentor.edition == CardEdition.Base)
            {
                baseMentors.Add(index);
            }
            index++;
        }

        //  Disable if no base mentors available
        if (baseMentors.Count == 0)
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

    //  25% chance to add an Edition to random Mentor
    public override void applyCardBuff ()
    {
        if(mentorHolder == null)
        {
            mentorHolder = GameObject.FindFirstObjectByType<JokerCardHolder>();
        }

        System.Random rand = new System.Random();

        if (rand.NextDouble() < 0.25)
        {
            int mentorIndex = baseMentors[rand.Next(baseMentors.Count)];

            // +1 because can't be base; also no weighing here
            CardEdition randEdition = (CardEdition) (1 + rand.Next(Enum.GetValues(typeof(CardEdition)).Length));
            player.mentorDeck[mentorIndex].edition = randEdition;
            mentorHolder.cards[mentorIndex].AssignMentor(player.mentorDeck[mentorIndex]);
        }

        //  Set prev used consumable to current consumable
        game.previousConsumable = CardBuffFactory(name);
    }
}

