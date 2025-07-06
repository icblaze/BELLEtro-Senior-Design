// This Document contains the code for the Turtle mentor
// Hand Size increase by 5 and reduce size by 1 each round.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Turtle : Mentor
{
    int handSizeBonus = 5;
    bool startRound = true;
    Player player = Player.access();
    Game game = Game.access();

    //  Mentor name and basePrice are preset
    public Turtle(CardEdition edition) : base(MentorName.Turtle, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Blind, UseLocation.PostBlind };
        description = "Hand Size increase by " + handSizeBonus + ", reduce by 1 end of round.";
    }

    //  Hand Size increase by 5 and reduce size by 1 each round.
    public override void UseMentor()
    {
        if(startRound)
        {
            //  Before intial draw to deck call
            player.handSize += player.handSize + handSizeBonus;

            //  Set flag for when called PostBlind
            startRound = false;
        }
        else
        {
            //  Reset hand size
            player.handSize -= handSizeBonus;

            //  Reduce by 1 end of round
            handSizeBonus--;

            //  Remove the mentor if bonus is depleted
            if(handSizeBonus == 0)
            {
                player.mentorDeck.Remove(this);

                //  TODO remove Mentor visually
            }

            //  Set flag for when called Blind
            startRound = true;
        }
        
    }
}
