// This document contains the code for the Mega Special Blind.
// This blind will be 6 times the score of the small blind.
// Current Devs:
// Fredrick Bouloute (bouloutef04)
using UnityEngine;

public class MegaBlind : SpecialBlind
{
    public MegaBlind() : base(SpecialBlindNames.MegaBlind, 6, 8)
    {
        description = "The name speaks for itself";
        nameText = "Mega Blind";
    }
    public override void applySpecialBlinds()
    {
        minimumAnte = 8;
        chipMultiplier = 6;
    }
    public override void cleanUpEffect()
    {
        //Still nothing special here
    }
}
