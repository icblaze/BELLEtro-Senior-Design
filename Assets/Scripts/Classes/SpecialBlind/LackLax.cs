// This document contains the code for the LackLax Special Blind.
// This blind will disabled Lax cards
// Current Devs:
// Andy (flakkid)

using System.Collections;
using UnityEngine;

public class LackLax : SpecialBlind
{
    HorizontalCardHolder horizontalCardHolder;

    public LackLax() : base(SpecialBlindNames.LackLax, 2, 1)
    {
        description = "Lax cards are disabled";
        nameText = "Lack Lax";
    }

    //  Set disabled suit flag and suit name (Lax) so when PCard's are assigned to Card object, they will be disabled
    public override void applySpecialBlinds()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(true, SuitName.Lax);
    }

    //  Re-enable disabled cards in card piles
    public override void cleanUpEffect()
    {
        horizontalCardHolder = GameObject.FindFirstObjectByType<HorizontalCardHolder>();
        horizontalCardHolder.SetDisabledSuit(false, SuitName.Lax);
    }
}
