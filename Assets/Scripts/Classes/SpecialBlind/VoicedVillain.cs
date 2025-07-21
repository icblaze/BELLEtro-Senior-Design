// This document contains the code for the VoicedVillain Special Blind.
// This blind will disabled Voiced cards
// Current Devs:
// Andy (flakkid)

using System.Collections;
using UnityEngine;

public class VoicedVillain : SpecialBlind
{
    HorizontalCardHolder horizontalCardHolder;

    public VoicedVillain() : base(SpecialBlindNames.VoicedVillain, 2, 1)
    {
        description = "Voiced cards are disabled";
        nameText = "Voiced Villain";
    }

    //  Set disabled suit flag and suit name (Voiced) so when PCard's are assigned to Card object, they will be disabled
    public override void applySpecialBlinds()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(true, SuitName.Voiced);
    }

    //  Re-enable disabled cards in card piles
    public override void cleanUpEffect()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(false, SuitName.Voiced);
    }
}
