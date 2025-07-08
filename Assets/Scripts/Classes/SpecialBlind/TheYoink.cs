// This document contains the code for the ThePurist Special Blind.
// This blind will set the hand count to 1 at the beginning of the round.
// Current Devs:
// Robert (momomonkeyman)

using System.Collections;
using UnityEngine;

public class TheYoink : SpecialBlind
{

    public TheYoink() : base(SpecialBlindNames.TheYoink, 2, 2)
    {
        description = "Every Hand or Discard you take will cost $1";
        nameText = "The Yoink";
    }

    public override void applySpecialBlinds()
    {
        GameObject.FindFirstObjectByType<PlayHand>().cashPenalty = true;
        GameObject.FindFirstObjectByType<DeleteCard>().cashPenalty = true;
    }

    public override void cleanUpEffect()
    {
        GameObject.FindFirstObjectByType<PlayHand>().cashPenalty = false;
        GameObject.FindFirstObjectByType<DeleteCard>().cashPenalty = false;
    }
}
