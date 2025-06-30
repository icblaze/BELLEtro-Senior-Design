// This Document contains the code for the EvenAirflow mentor
// This Mentor effect is X2 Mult if played hand has both voiced and voiceless sounds scored.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EvenAirflow : Mentor
{
    Player player = Player.access();

    //  Mentor name and basePrice are preset
    public EvenAirflow(CardEdition edition) : base(MentorName.EvenAirflow, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Post};
        description = "X2 Mult if played hand has both voiced and voiceless sounds scored.";
    }

    //  X2 Mult if played hand has both voiced and voiceless sounds scored.
    public override void UseMentor()
    {
        bool hasVoiced = false;
        bool hasVoiceless = false;

        List<PCard> pcardList = GameObject.FindFirstObjectByType<DeleteCard>().GetSelectedPCards();

        foreach(PCard pcard in pcardList)
        {
            if(pcard.suit == SuitName.Voiced || pcard.enhancement == CardEnhancement.WildCard)
            {
                hasVoiced = true;
            }
            if (pcard.suit == SuitName.Voiceless || pcard.enhancement == CardEnhancement.WildCard)
            {
                hasVoiceless = true;
            }
        }

        //  TODO Multiply the Round mult variable by 2
        if(hasVoiced && hasVoiceless)
        {

        }
        
    }

}
