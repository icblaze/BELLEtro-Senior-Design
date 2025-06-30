// This Document contains the code for the NoVoice mentor
// Effect is to have played cards with Voiceless/Lax suit give +3 Mult when scored
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class NoVoice : Mentor
{
    //  Mentor name and basePrice are preset
    public NoVoice(CardEdition edition) : base(MentorName.NoVoice, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.PreCard };
        description = "Played cards with Voiceless/Lax suit give +3 Mult when scored";
    }

    //  Played card will add +3 Mult if Voiceless/Lax suit
    public override void UseMentor(PCard card)
    {
        if(card.suit == SuitName.Voiceless || card.enhancement == CardEnhancement.WildCard)
        {
            Debug.Log("+3 Mult since Voiceless/Lax");
            //  TODO add +3 to Mult variable of round
        }
    }
}
