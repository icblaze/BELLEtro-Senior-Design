// This Document contains the code for the CheatSheet Mentor.
// Effect is to copy the effect of the Mentor to its right.
// Current Devs:
// Andy (flakkid): made format for constructor and overridden effect method, singleton change

using UnityEngine;
using System.Collections;


public class CheatSheet : Mentor
{
    // Stores the rightMentor
    Mentor rightMentor = null;

    //  Mentor name and basePrice are preset
    public CheatSheet(CardEdition edition) : base(MentorName.CheatSheet, edition, 10)
    {
        //  Normally assign locations here, but CheatSheet copies Mentor to the right
        description = "Copies the ability of the Mentor to the right";
    }

    //  Dynamically change the effect of Cheat Sheet based on rightMentor, call in JokerCard group
    public void ChangeEffect ()
    {
        Player player = Player.access();

        //  Get index of Cheat Sheet mentor
        int currIndex = player.mentorDeck.IndexOf(this);

        //  If valid mentor to the right, assign it and change its locations
        if (currIndex < player.mentorDeck.Count - 1)
        {
            Mentor rightMentor = player.mentorDeck[currIndex + 1];
            locations = rightMentor.locations;
        }
    }

    //  Uses rightMentor effect
    public override void UseMentor()
    {
        rightMentor.UseMentor();
    }
}
