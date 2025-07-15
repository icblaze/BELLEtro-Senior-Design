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
    public int minimumAnte;
    public string description;
    public string nameText;

    public SpecialBlind(SpecialBlindNames specialBlindName, float mult, int minAnte)
    {
        //Complete this constructor for setting up the Special Blind
        this.blindType = specialBlindName;
        this.chipMultiplier = mult;
        this.minimumAnte = minAnte;
    }

    public virtual void applySpecialBlinds()
    {

    }

    public virtual void cleanUpEffect()
    {

    }

    public static SpecialBlind BlindFactory(SpecialBlindNames name)
    {
        switch(name)
        {
            case SpecialBlindNames.TheCollector:
                return new TheCollector();
            case SpecialBlindNames.ThePurist:
                return new ThePurist();
            case SpecialBlindNames.TheYoink:
                return new TheYoink();
            case SpecialBlindNames.TheSilence:
                return new TheSilence();
            case SpecialBlindNames.JumboBlind:
                return new JumboBlind();
            case SpecialBlindNames.MegaBlind:
                return new MegaBlind();
            default:
                return null;
        }
    }

    public static SpecialBlind BlindFromNumber(int index)
    {
        switch(index)
        {
            case 0:
                return BlindFactory(SpecialBlindNames.TheCollector);
            case 1:
                return BlindFactory(SpecialBlindNames.ThePurist);
            case 2:
                return BlindFactory(SpecialBlindNames.TheYoink);
            case 3:
                return BlindFactory(SpecialBlindNames.TheSilence);
            case 4:
                return BlindFactory(SpecialBlindNames.JumboBlind);
            case 5:
                return BlindFactory(SpecialBlindNames.MegaBlind);
            default:
                return null;
        }
    }
}
