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
        //Nothing special here
    }
    public override void cleanUpEffect()
    {
        //Still nothing special here
    }
}
