// This Document contains the code for the CheatSheet Mentor.
//  Effect is to copy the effect of the Mentor to its right.
// Current Devs:
// Andy (flakkid): made format for constructor and overridden effect method

using UnityEngine;
using System.Collections;


public class CheatSheet : Mentor
{
    //  Stores the rightMentor
    Mentor rightMentor = null;

    //  Mentor name and basePrice are preset
    public CheatSheet(CardEdition edition) : base(MentorName.CheatSheet, edition, 10)
    {
        //  Normally assign locations here, but CheatSheet copies Mentor to the right
    }

    //  Dynamically change the effect of Cheat Sheet based on rightMentor
    public void ChangeEffect (Game game)
    {
        //  Get index of Cheat Sheet mentor
        int currIndex = game.ThePlayer.mentorDeck.IndexOf(this);

        //  If valid mentor to the right, assign it and change its locations
        if (currIndex < game.ThePlayer.mentorDeck.Count - 1)
        {
            Mentor rightMentor = game.ThePlayer.mentorDeck[currIndex + 1];
            locations = rightMentor.locations;
        }
    }

    //  Uses rightMentor effect
    public override void UseMentor(Game game)
    {
        rightMentor.UseMentor(game);
    }
}
