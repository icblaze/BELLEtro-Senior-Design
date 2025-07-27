// This Document contains the code for the FastLearner mentor
// Effect is +1 Mult for every 7 cards discarded this run (+0 Mult)
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class FastLearner : Mentor
{
    int discardCounter = 0;
    int mult = 0;
    DeleteCard deleteScript;

    //  Mentor name and basePrice are preset
    public FastLearner(CardEdition edition) : base(MentorName.FastLearner, edition, 5)
    {
        locations = new UseLocation[] { UseLocation.Discard, UseLocation.Post };
        description = "Mentor gains <color=red>+1 Mult</color> for every 7 cards discarded this run (<color=red>+0 Mult</color>)";
    }

    //  Change mult in description
    public override string GetDescription()
    {
        description = $"Mentor gains <color=red>+1 Mult</color> for every 7 cards discarded this run (<color=red>+{mult} Mult</color>)";
        return description;
    }

    // Mentor gains +1 Mult for every 7 cards discarded this run
    public override void UseMentor()
    {
        if(deleteScript == null)
        {
            deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        }

        if(ScoringManager.access().GetScoringStatus())
        {
            ScoringManager.access().IncrementCurrentMult(mult);
        }
        else
        {
            //  If not in middle of scoring, increment mult based on condition
            int cardCount = deleteScript.GetSelectedPCards().Count;

            discardCounter += cardCount;

            //  Rollover counter after every 7, and increment mult
            if(discardCounter >= 7)
            {
                discardCounter -= 7;
                mult++;
            }
        }
    }
}
