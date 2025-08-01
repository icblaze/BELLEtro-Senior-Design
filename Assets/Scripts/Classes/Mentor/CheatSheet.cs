﻿// This Document contains the code for the CheatSheet Mentor.
// Effect is to copy the effect of the Mentor to its right.
// Current Devs:
// Andy (flakkid): made format for constructor and overridden effect method, singleton change

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class CheatSheet : Mentor
{
    // Stores the rightMentor
    Mentor rightMentor = null;

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
    public CheatSheet(CardEdition edition) : base(MentorName.CheatSheet, edition, 10)
    {
        //  Normally assign locations here, but CheatSheet copies Mentor to the right
        description = "Copies the ability of the Mentor to the right";
    }

    public override string GetDescription()
    {
        description = "Copies the ability of the Mentor to the right";

        if (rightMentor == null)
        {
            description += " (none)";
            return description;
        }
        else if (incompatibleMentors.Contains(rightMentor.name))
        {
            description += " (incompatible)";
            return description;
        }

        description += " (compatible)";
        return description;
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
            rightMentor = player.mentorDeck[currIndex + 1];
            if(incompatibleMentors.Contains(rightMentor.name))
            {
                locations = new UseLocation[] { };
            }
            else
            {
                locations = rightMentor.locations.ToArray();
            }
        }
        else
        {
            rightMentor = null;
            locations = new UseLocation[]{ };
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

    //  Uses leftmostMentor's effect when retrigger effect
    public override void UseRetriggerMentor(List<PCard> scoredPCards)
    {
        if (rightMentor == null)
        {
            return;
        }
        else if (incompatibleMentors.Contains(rightMentor.name))
        {
            return;
        }
        rightMentor.UseRetriggerMentor(scoredPCards);
    }
}
