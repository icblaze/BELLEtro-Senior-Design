// This Document contains the code for the HandsOff Tag.
// This tag will reward a player with 1 currency per hand played.
// Current Devs:
// Fredrick Bouloute (bouloutef04)
using UnityEngine;

public class HandsOn :Tag
{
    public HandsOn() : base(TagNames.HandsOn)
    {
        description = "Gives $1 for each hand played";
        tagName = "Hands On";
    }

    public override void applyTag()
    {
        Player.access().moneyCount += Player.access().handsPlayed;
    }
}
