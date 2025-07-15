using UnityEngine;

public class TheHandful : SpecialBlind
{
    public TheHandful() : base(SpecialBlindNames.TheHandful, 2, 2)
    {
        description = "Play only 5 cards per hand";
        nameText = "The Handful";
    }

    public override void applySpecialBlinds()
    {
        GameObject.FindFirstObjectByType<PlayHand>().handful = true;
    }

    public override void cleanUpEffect()
    {
        GameObject.FindFirstObjectByType<PlayHand>().handful = false;
    }
}
