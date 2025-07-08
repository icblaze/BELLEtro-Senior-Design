// This Document contains the code for the NegativeMentor Tag.
// This tag will reward a player with a negative mentor at the next shop.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

public class NegativeMentor : Tag
{
    public NegativeMentor() : base(TagNames.NegativeMentor)
    {
        description = "At the next Shop, the first Mentor will become negative";
        tagName = "Negative Mentor";
    }

    public override void applyTag()
    {

    }
}
