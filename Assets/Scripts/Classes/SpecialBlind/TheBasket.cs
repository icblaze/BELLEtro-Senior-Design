using UnityEngine;

public class TheBasket : SpecialBlind
{
        public TheBasket() : base(SpecialBlindNames.TheBasket, 2, 2)
    {
        description = "Draw three cards every hand or discard";
        nameText = "The Basket";
    }

    public override void applySpecialBlinds()
    {
        GameObject.FindFirstObjectByType<PlayHand>().drawThree = true;
        GameObject.FindFirstObjectByType<DeleteCard>().drawThree = true;
    }

    public override void cleanUpEffect()
    {
        GameObject.FindFirstObjectByType<PlayHand>().drawThree = false;
        GameObject.FindFirstObjectByType<DeleteCard>().drawThree = false;
    }
}
