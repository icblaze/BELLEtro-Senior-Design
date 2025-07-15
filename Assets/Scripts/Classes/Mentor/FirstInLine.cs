// This Document contains the code for the FirstInLine mentor
// This Mentor gives X2 Mult when Diphthong is scored
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class FirstInLine : Mentor
{
    //  Mentor name and basePrice are preset
    public FirstInLine(CardEdition edition) : base(MentorName.FirstInLine, edition, 4)
    {
        locations = new UseLocation[] {UseLocation.Retrigger};
        description = "The first scored card in hand will be retriggered an additional 2 times";
    }

    //  The first scored card in hand will be retriggered an additional 2 times
    public override void UseRetriggerMentor(List<PCard> scoredPCards)
    {
        //  Only increment replay counter by 2 for first scored card
        scoredPCards[0].replayCounter += 2;
    }
}
