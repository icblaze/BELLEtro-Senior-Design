// This Document contains the code for the CheatSheet Mentor.
// Effect is to copy the effect of the Mentor to its right.
// Current Devs:
// Andy (flakkid): made format for constructor and overridden effect method, singleton change

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CheatSheet : Mentor
{
    // Stores the rightMentor
    Mentor rightMentor = null;

    //  Incompatible mentors for copying
    private static readonly HashSet<MentorName> incompatibleMentors = new()
    {
        MentorName.TwelveCredits,
        MentorName.Turtle,
        MentorName.Astronaut,
        MentorName.Curve,
        MentorName.Extension,
        MentorName.HelpingHand,
        MentorName.LibraryCard
    };

    //  Mentor name and basePrice are preset
    public CheatSheet(CardEdition edition) : base(MentorName.CheatSheet, edition, 10)
    {
        //  Normally assign locations here, but CheatSheet copies Mentor to the right
        description = "Copies the ability of the Mentor to the right";
    }

    public override string GetDescription()
    {
        description = "Copies the ability of the Mentor to the Mentor";

        if (rightMentor == null)
        {
            description += " (none)";
        }
        else if (incompatibleMentors.Contains(rightMentor.name))
        {
            description += " (incompatible)";
        }

        description += " (compatible)";
        return description;
    }

    //  Dynamically change the effect of Cheat Sheet based on rightMentor, call in JokerCard group
    public void ChangeEffect ()
    {
        //Debug.Log("mic check cheat sheet");
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
        if(rightMentor == null)
        {
            return;
        }
        else if (incompatibleMentors.Contains(rightMentor.name))
        {
            return;
        }
        rightMentor.UseMentor();
    }

    //  Uses rightMentor effect when card-specific
    public override void UseMentor(PCard card)
    {
        if (rightMentor == null)
        {
            return;
        }
        else if (incompatibleMentors.Contains(rightMentor.name))
        {
            return;
        }
        rightMentor.UseMentor(card);
    }
}
