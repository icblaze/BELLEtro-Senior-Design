using UnityEngine;

public class Shoplifter : Tag
{
    public Shoplifter() : base(TagNames.Shoplifter)
    {
        description = "Next Shop reroll is free";
        tagName = "Shoplifter";
    }

    public override void applyTag()
    {
        ShopManager shopManager = GameObject.Find("ShopManager").GetComponent<ShopManager>();
        shopManager.freeReroll = true;
    }
}
