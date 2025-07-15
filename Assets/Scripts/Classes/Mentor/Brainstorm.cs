// This Document contains the code for the Brainstorm Mentor.
// Effect is to copy the effect of the leftmost Mentor
// Current Devs:
// Andy (flakkid): made format for constructor and overridden effect method, singleton change

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class Brainstorm : Mentor
{
    // Stores the leftmost Mentor
    Mentor leftmostMentor = null;

    //  Incompatible mentors for copying
    private static readonly HashSet<MentorName> incompatibleMentors = new()
    {
        MentorName.Turtle,
        MentorName.Astronaut,
        MentorName.Curve,
        MentorName.Extension,
        MentorName.HelpingHand,
        MentorName.LibraryCard,
        MentorName.PageFlip,
        MentorName.Overachiever,
        MentorName.Revisionist,
        MentorName.Recess,
        MentorName.Bookstore,
        MentorName.LightSnack
    };

    //  Mentor name and basePrice are preset
    public Brainstorm(CardEdition edition) : base(MentorName.Brainstorm, edition, 10)
    {
        //  Normally assign locations here, but CheatSheet copies Mentor to the right
        description = "Copies the ability of the leftmost Mentor";
    }

    public override string GetDescription()
    {
        ChangeEffect();
        description = "Copies the ability of the leftmost Mentor";

        if(leftmostMentor == null)
        {
            description += " (none)";
            return description;
        }
        else if(incompatibleMentors.Contains(leftmostMentor.name))
        {
            description += " (incompatible)";
            return description;
        }

        description += " (compatible)";
        return description;
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
            leftmostMentor = player.mentorDeck[0];
            if (incompatibleMentors.Contains(leftmostMentor.name))
            {
                locations = new UseLocation[] { };
            }
            else
            {
                locations = leftmostMentor.locations.ToArray();
            }
        }
        else
        {
            leftmostMentor = null;
            locations = new UseLocation[] { };
        }
    }

    //  Uses leftmostMentor's effect
    public override void UseMentor()
    {
        if(leftmostMentor == null)
        {
            return;
        }
        else if(incompatibleMentors.Contains(leftmostMentor.name))
        {
            return;
        }
        leftmostMentor.UseMentor();
    }

    //  Uses leftmostMentor's effect when card-specific
    public override void UseMentor(PCard card)
    {
        if (leftmostMentor == null)
        {
            return;
        }
        else if (incompatibleMentors.Contains(leftmostMentor.name))
        {
            return;
        }
        leftmostMentor.UseMentor(card);
    }

    //  Uses leftmostMentor's effect when retrigger effect
    public override void UseRetriggerMentor(List<PCard> scoredPCards)
    {
        if (leftmostMentor == null)
        {
            return;
        }
        else if (incompatibleMentors.Contains(leftmostMentor.name))
        {
            return;
        }
        leftmostMentor.UseRetriggerMentor(scoredPCards);
    }
}
