// This Document contains the code for the Diphcotomy mentor
// This Mentor gains +1 Mult per consecutive hand played without a scoring Diphthong"
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Diphcotomy : Mentor
{
    int mult = 0;

    //  Mentor name and basePrice are preset
    public Diphcotomy(CardEdition edition) : base(MentorName.Diphcotomy, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.PreCard, UseLocation.Post };
        description = "This Mentor gains +1 Mult per consecutive hand played without a scoring Diphthong ( +0)";
    }

    //  Update description with mult variable
    public override string GetDescription()
    {
        description = "This Mentor gains +1 Mult per consecutive hand played without a scoring Diphthong ( +" + mult + ")";
        return description;
    }

    //  Will reset mult to 0 if pcard isDiphthong (PreCard)
    public override void UseMentor(PCard pcard)
    {
        if(pcard.isDiphthong)
        {
            mult = 0;
        }
    }

    //  TODO Add +1 Mult to mult variable, and add mult to round variable (Post)
    public override void UseMentor()
    {
        mult++;

        //  TODO Add mult to round variable
    }
}
