// This Document contains the code for the HandsOff Tag.
// This tag will reward a player with 1 currency per discard used.
// Current Devs:
// Fredrick Bouloute (bouloutef04)
using UnityEngine;

public class HandsOff : Tag
{
    public HandsOff() : base(TagNames.HandsOff)
    {
        description = "Gives $1 for each discard used";
        tagName = "Hands Off";
    }

    public override void applyTag()
    {
        Player.access().moneyCount += Player.access().discardsPlayed;
    }
}
