// This document contains the code for the Jumbo Special Blind.
// This blind will be 4 times the score of the small blind.
// Current Devs:
// Fredrick Bouloute (bouloutef04)
using UnityEngine;

public class JumboBlind : SpecialBlind
{
    public JumboBlind() : base(SpecialBlindNames.JumboBlind, 4, 2)
    {
        description = "Very Big Blind";
        nameText = "Jumbo Blind";
    }
    public override void applySpecialBlinds()
    {
        //Nothing special here
    }
    public override void cleanUpEffect()
    {
        //Still nothing special here
    }
}
