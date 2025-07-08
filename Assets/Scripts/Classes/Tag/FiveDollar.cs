// This Document contains the code for the FiveDollar Tag.
// This tag will reward a player with 5 currency for skipping a blind.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

public class FiveDollar : Tag
{
    public FiveDollar() : base(TagNames.FiveDollar)
    {
        description = "Gives $5";
        tagName = "More Money";
    }

    public override void applyTag()
    {
        Player.access().moneyCount += 5;
    }
}
