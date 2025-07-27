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
        description = "This Mentor gains <color=red>+1 Mult</color> per consecutive hand played without a scoring Diphthong (<color=red>+0 Mult</color>)";
    }

    //  Update description with mult variable
    public override string GetDescription()
    {
        description = "This Mentor gains <color=red>+1 Mult</color> per consecutive hand played without a scoring Diphthong (<color=red>+" + mult + " Mult</color>)";
        return description;
    }

    //  Will reset mult to -1 if pcard isDiphthong (PreCard)
    public override void UseMentor(PCard pcard)
    {
        if(pcard.isDiphthong)
        {
            mult = -1;  //  so it'll be +0 when Post is run
        }
    }

    //  Add +1 Mult to mult variable, and add mult to round variable (Post)
    public override void UseMentor()
    {
        //  Increment before adding to mult
        mult++;

        ScoringManager.access().IncrementCurrentMult(mult);
    }
}
