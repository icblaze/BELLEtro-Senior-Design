// This Document contains the code for the SkipMoney Tag.
// This tag will reward a player with 5 currency for each time they have skipped.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

public class SkipMoney : Tag
{
    public SkipMoney() : base(TagNames.SkipMoney)
    {
        description = "Gives $5 for each skipped Blind";
        tagName = "Skipped Dues";
    }

    public override void applyTag()
    {
        Player.access().moneyCount += 5 * Game.access().skipCount;
    }
}
