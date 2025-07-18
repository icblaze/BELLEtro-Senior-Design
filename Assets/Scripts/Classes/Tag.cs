// This Document contains the code for the Tag class.
// This class holds information on Tags which are used as rewards
// for players to earn whenever they skip a Blind
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

public class Tag
{
    public string tagName;
    public string description;
    public TagNames name;

    public Tag(TagNames name)
    {
        this.name = name;
    }

    public static Tag TagFactory(TagNames name)
    {
        switch (name)
        {
            case TagNames.FiveDollar:
                return new FiveDollar();
            case TagNames.SkipMoney:
                return new SkipMoney();
            case TagNames.Shoplifter:
                return new Shoplifter();
            case TagNames.HandsOn:
                return new HandsOn();
            case TagNames.HandsOff:
                return new HandsOff();
            default:
                return null;
        }
    }

    public static Tag TagFromIndex(int index)
    {
        switch (index)
        {
            case 0:
                return TagFactory(TagNames.FiveDollar);
            case 1:
                return TagFactory(TagNames.SkipMoney);
            case 2:
                return TagFactory(TagNames.Shoplifter);
            case 3:
                return TagFactory(TagNames.HandsOn);
            case 4:
                return TagFactory(TagNames.HandsOff);
            default:
                return null;
        }
    }

    // Activates the effect of the tag (more information is provided in each specific Tag.
    public virtual void applyTag()
    {

    }
}
