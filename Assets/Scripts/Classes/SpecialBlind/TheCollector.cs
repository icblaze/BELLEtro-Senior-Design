// This document contains the code for the TheCollector Special Blind.
// This blind will set the discards to 0 at the beginning of the round.
// Current Devs:
// Robert (momomonkeyman)

using System.Collections;
using UnityEngine;

public class TheCollector : SpecialBlind
{
    private int OriginalDiscards;

    public TheCollector() : base(SpecialBlindNames.TheCollector, 2, 2)
    {
        description = "Start with 0 discards";
        nameText = "The Collector";
    }

    public override void applySpecialBlinds()
    {
        OriginalDiscards = Player.access().maxDiscards;

        Player.access().maxDiscards = 0;
    }

    public override void cleanUpEffect()
    {
        Player.access().maxDiscards = OriginalDiscards;
    }
}

