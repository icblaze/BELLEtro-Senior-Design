// This Document contains the code for the Turtle mentor
// Hand Size increase by 5 and reduce size by 1 each round.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Turtle : Mentor
{
    public int handSizeBonus = 5;
    public bool bonusApplied = false;   //  One-time flag
    bool startRound = true;
    Player player = Player.access();
    Game game = Game.access();
    JokerCardHolder mentorCardHolder;

    //  Mentor name and basePrice are preset
    public Turtle(CardEdition edition) : base(MentorName.Turtle, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Blind, UseLocation.PostBlind };
        description = "Hand Size increase by <color=orange>" + handSizeBonus + "</color>, reduce by <color=orange>1</color> end of round.";
    }

    //  Update handSize bonus in description
    public override string GetDescription()
    {
        description = "Hand Size increase by <color=orange>" + handSizeBonus + "</color>, reduce by <color=orange>1</color> end of round.";
        return description;
    }

    //  Hand Size increase by 5 and reduce size by 1 each round.
    public override void UseMentor()
    {
        if(startRound)
        {
            Debug.Log("Turtle Hand Size");
            //  Before intial draw to deck call
            player.handSize += handSizeBonus;
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=orange>+{handSizeBonus} Hand Size</color>"));

            //  Set flag for when called PostBlind
            startRound = false;

            //  Indicate that handSizeBonus has been applied
            bonusApplied = true;
        }
        else
        {
            Debug.Log("Turtle Decrease");
            //  Reset hand size
            player.handSize -= handSizeBonus;

            //  Reduce by 1 end of round
            handSizeBonus--;

            //  Remove the mentor if bonus is depleted
            if(handSizeBonus <= 0)
            {
                mentorCardHolder.RemoveMentor(this);
            }
            else
            {
                ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=orange>-1 Hand Size</color>"));
            }

            //  Set flag for when called Blind
            startRound = true;
        }
        
    }
}
