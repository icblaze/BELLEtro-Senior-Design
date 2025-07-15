// This Document contains the code for the Library mentor
// Effect is this mentor gains x0.1 Mult every time a Textbook is used (X1 Mult)
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System;

public class Library : Mentor
{
    private float textbookMult = 1;

    //  Mentor name and basePrice are preset
    public Library(CardEdition edition) : base(MentorName.Library, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "This mentor gains x0.1 Mult when a Textbook is used (X1 Mult)";
    }

    //  This mentor gains X0.1 Mult when a Textbook is used (X1 Mult)
    public override string GetDescription()
    {
        description = $"This mentor gains X0.1 Mult when a Textbook is used (X{textbookMult} Mult)";
        return description;
    }

    public override void UseMentor()
    {
        int newMult = (int) Math.Ceiling(ScoringManager.access().GetCurrentMult() * textbookMult);
        ScoringManager.access().SetCurrentMult(newMult);
    }

    //  Called when textbook is used
    public void IncreaseMult()
    {
        textbookMult += 0.1f;
    }
}
