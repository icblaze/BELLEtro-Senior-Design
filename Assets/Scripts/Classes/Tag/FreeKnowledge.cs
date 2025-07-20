using UnityEngine;
using UnityEngine.UI;

public class FreeKnowledge : Tag
{
    public FreeKnowledge() : base(TagNames.FreeKnowledge)
    {
        description = "Obtain a free Card Buff";
        tagName = "Free Knowledge";
    }

    public override void applyTag()
    {
        //If there is space
        ConsumableCardHolder consumableHolder = GameObject.FindFirstObjectByType<ConsumableCardHolder>();
        if (consumableHolder != null && (Player.access().consumables.Count < Player.access().maxConsumables))
        {
            consumableHolder.AddConsumable(Game.access().randomCardBuffShop(1)[0]);
            return;
        }
        SFXManager sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
        ShakeScreen shakeScreen = GameObject.FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
        sfxManager.NoSFX();
        shakeScreen.StartShake();
    }
}
