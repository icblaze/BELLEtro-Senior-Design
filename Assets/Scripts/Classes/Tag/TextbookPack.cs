// This Document contains the code for the Textbook Tag.
// This tag will reward a player with a textbook pack.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

public class TextbookPack : Tag
{
    public TextbookPack() : base(TagNames.TextbookPack)
    {
        description = "Pack of Textbook Cards. There are 5 cards and 2 can be chosen.";
        tagName = "Textbook Pack";
    }

    public override void applyTag()
    {

    }
}
