// This Document contains the code for the Astronaut mentor
// Effect is that Played Consonants give plus 30 chips when scored.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Consonant : Mentor
{
    

    //  Mentor name and basePrice are preset
    public Consonant(CardEdition edition) : base(MentorName.Consonant, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.PreCard };
        description = " Consonants give +30 Chips when scored";
    }

    //  Played Consonants give plus 30 chips when scored.
    public override void UseMentor(PCard pcard)
    {
        //  Terms 1-26 are Consonants
        for(int i = 1; i <= 26; i++)
        {
            //  Check if pcard is consonant
            if ((LinguisticTerms) i == pcard.term)
            {
                Debug.Log("+30 cuz consonant");
                ScoringManager.access().IncrementCurrentChips(30);
            }
        }
    }
}
