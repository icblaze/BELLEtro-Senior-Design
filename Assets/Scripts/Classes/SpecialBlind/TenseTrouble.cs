// This document contains the code for the TenseTrouble Special Blind.
// This blind will disabled Tense cards
// Current Devs:
// Andy (flakkid)

using System.Collections;
using UnityEngine;

public class TenseTrouble : SpecialBlind
{
    HorizontalCardHolder horizontalCardHolder;

    public TenseTrouble() : base(SpecialBlindNames.TenseTrouble, 2, 1)
    {
        description = "Tense cards are disabled";
        nameText = "Tense Trouble";
    }

    //  Set disabled suit flag and suit name (Tense) so when PCard's are assigned to Card object, they will be disabled
    public override void applySpecialBlinds()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(true, SuitName.Tense);
    }

    //  Re-enable disabled cards in card piles
    public override void cleanUpEffect()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(false, SuitName.Tense);
    }
}
