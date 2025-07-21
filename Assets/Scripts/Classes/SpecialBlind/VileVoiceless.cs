// This document contains the code for the VileVoiceless Special Blind.
// This blind will disable Voiceless cards
// Current Devs:
// Andy (flakkid)

using System.Collections;
using UnityEngine;

public class VileVoiceless : SpecialBlind
{
    HorizontalCardHolder horizontalCardHolder;

    public VileVoiceless() : base(SpecialBlindNames.VileVoiceless, 2, 1)
    {
        description = "Voiceless cards are disabled";
        nameText = "Vile Voiceless";
    }

    //  Set disabled suit flag and suit name (Voiceless) so when PCard's are assigned to Card object, they will be disabled
    public override void applySpecialBlinds()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(true, SuitName.Voiceless);
    }

    //  Re-enable disabled cards in card piles
    public override void cleanUpEffect()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(false, SuitName.Voiceless);
    }
}
