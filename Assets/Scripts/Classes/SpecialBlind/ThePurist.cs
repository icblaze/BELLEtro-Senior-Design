// This document contains the code for the ThePurist Special Blind.
// This blind will set the hand count to 1 at the beginning of the round.
// Current Devs:
// Robert (momomonkeyman)

using System.Collections;
using UnityEngine;

public class ThePurist : SpecialBlind
{
    private int OriginalHandCount;

    public ThePurist() : base(SpecialBlindNames.ThePurist, 1, 2)
    {
        description = "Play a single hand";
        nameText = "The Purist";
    }

    public override void applySpecialBlinds()
    {
        OriginalHandCount = Player.access().handCount;

        Player.access().handCount = 0;
    }

    public override void cleanUpEffect()
    {
        Player.access().handCount = OriginalHandCount;
    }
}
