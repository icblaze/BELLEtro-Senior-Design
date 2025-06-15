// This Document contains the code for the SpecialBlind class.
// This class holds information on a special blind. It is mainly used
// to set up the Round state or to make sure that none of the previous 
// special blinds are reused later.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Zacharia Alaoui (ZachariaAlaoui): Made constructor for the SpecialBlind class

using System.Collections;
using UnityEngine;

public class SpecialBlind
{
    public float chipMultiplier;
    public SpecialBlindNames blindType;

    public SpecialBlind(SpecialBlindNames specialBlindName)
    {
        //Complete this constructor for setting up the Special Blind
        this.blindType = specialBlindName;
    }

    public void applySpecialBlinds()
    {

    }
}
