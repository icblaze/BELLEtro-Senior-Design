// This Document contains the code for the Brainstorm Mentor.
// Effect is to copy the effect of the leftmost Mentor
// Current Devs:
// Andy (flakkid): made format for constructor and overridden effect method, singleton change

using UnityEngine;
using System.Collections;


public class Brainstorm : Mentor
{
    // Stores the leftmost Mentor
    Mentor leftmostMentor = null;

    //  Mentor name and basePrice are preset
    public Brainstorm(CardEdition edition) : base(MentorName.Brainstorm, edition, 10)
    {
        //  Normally assign locations here, but CheatSheet copies Mentor to the right
        description = "Copies the ability of the leftmost Mentor";
    }

    //  Dynamically change the effect of Cheat Sheet based on mentorDeck, call in JokerCard group
    public void ChangeEffect ()
    {
        //Debug.Log("mic check brainstorm");
        Player player = Player.access();

        //  Get index of Brainstorm mentor
        int currIndex = player.mentorDeck.IndexOf(this);

        //  If there exists another valid mentor that is leftmost and not Brainstorm itself
        if (currIndex != 0 && player.mentorDeck.Count > 1)
        {
            Mentor leftmostMentor = player.mentorDeck[0];
            locations = leftmostMentor.locations;
        }
    }

    //  Uses leftmostMentor's effect
    public override void UseMentor()
    {
        leftmostMentor.UseMentor();
    }

    //  Uses leftmostMentor's effect when card-specific
    public override void UseMentor(PCard card)
    {
        leftmostMentor.UseMentor(card);
    }
}
