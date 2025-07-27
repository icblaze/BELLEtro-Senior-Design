// This Document contains the code for the MindMeld mentor
// Effect is Mentor gains +1 Mult if multiple hand types played this round (+0 Mult)
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MindMeld : Mentor
{
    private int mult = 0;
    private HashSet<string> handNames = new();

    //  Mentor name and basePrice are preset
    public MindMeld(CardEdition edition) : base(MentorName.MindMeld, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "Mentor gains <color=red>+1 Mult</color> if multiple hand types played this round (<color=red>+0 Mult</color>)";
    }

    //  Change mult of description
    public override string GetDescription()
    {
        description = $"Mentor gains <color=red>+1 Mult</color> if multiple hand types played this round (<color=red>+{mult} Mult</color>)";
        return description;
    }

    //  Mentor gains +1 Mult if different hand type played this round (+0 Mult)
    public override void UseMentor()
    {
        string hand = ScoringManager.access().GetCurrentHandType();

        //  If more than one hand type present in set
        if (handNames.Count > 0)
        {
            //  If not already played, then increment mult and add to set
            if(!handNames.Contains(hand))
            {
                handNames.Add(hand);
                mult++;
            }
        }
        else
        {
            handNames.Add(hand);    //  Add first hand type
        }

        ScoringManager.access().IncrementCurrentMult(mult);
    }

    //  Empty hash set
    public void EmptyHandSet()
    {
        handNames.Clear();
    }
}
