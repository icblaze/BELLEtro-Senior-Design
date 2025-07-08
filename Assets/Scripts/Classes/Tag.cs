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
          default:
              return null;
       }
    }
    
    // Activates the effect of the tag (more information is provided in each specific Tag.
    public virtual void applyTag()
    {

    }
}
