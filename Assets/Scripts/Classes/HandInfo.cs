// This Document contains the code for the HandInfo class
// This class contains information about a hand's level,
// their initial base score, and specific increase.

// Current Devs:
// Andy (flakkid): made class's constuctor, variables, getters


using UnityEngine;
using System.Collections;

[System.Serializable]
public class HandInfo : ScriptableObject
{
    private int baseChips;
    private int baseMult;

    public int level;
    public int incrementChips;
    public int incrementMult;

    //  Construct hand at level 1 with associated base and increment values
    public HandInfo(TextbookName textbookName)
    {
        level = 1;

        switch (textbookName)
        {
            case (TextbookName.HighCard):
                baseMult = 1;
                baseChips = 5;
                incrementMult = 1;
                incrementChips = 10;
                break;
            case (TextbookName.Pair):
                baseMult = 2;
                baseChips = 10;
                incrementMult = 1;
                incrementChips = 15;
                break;
            case (TextbookName.TwoPair):
                baseMult = 2;
                baseChips = 20;
                incrementMult = 1;
                incrementChips = 20;
                break;
            case (TextbookName.ThreeKind):
                baseMult = 3;
                baseChips = 30;
                incrementMult = 2;
                incrementChips = 20;
                break;
            case (TextbookName.Straight):
                baseMult = 4;
                baseChips = 30;
                incrementMult = 3;
                incrementChips = 30;
                break;
            case (TextbookName.Flush):
                baseMult = 4;
                baseChips = 35;
                incrementMult = 2;
                incrementChips = 15;
                break;
            case (TextbookName.FullHouse):
                baseMult = 4;
                baseChips = 40;
                incrementMult = 2;
                incrementChips = 25;
                break;
            case (TextbookName.FourKind):
                baseMult = 7;
                baseChips = 60;
                incrementMult = 3;
                incrementChips = 30;
                break;
            case (TextbookName.StraightFlush):
                baseMult = 8;
                baseChips = 100;
                incrementMult = 4;
                incrementChips = 40;
                break;
            case (TextbookName.FiveKind):
                baseMult = 12;
                baseChips = 120;
                incrementMult = 3;
                incrementChips = 35;
                break;
            case (TextbookName.FlushHouse):
                baseMult = 14;
                baseChips = 140;
                incrementMult = 4;
                incrementChips = 40;
                break;
            case (TextbookName.FlushFive):
                baseMult = 16;
                baseChips = 160;
                incrementMult = 3;  
                incrementChips = 50;
                break;
        }
    }

    //  Increase level of hand and scoring
    public void increaseLevel()
    {
        level++;
    }

    //  Decrease level of hand and scoring
    public bool decreaseLevel()
    {
        //  Can't decrease level below 1
        if (level == 1)
        {
            return false;
        }

        level--;
        return true;
    }

    //  Get the current chips (using base chips, increment, and level)
    public int getCurrChips()
    {
        int currChips = baseChips + (incrementChips * level);
        return currChips;
    }

    //  Get the current mult (using base mult, increment, and level)
    public int getCurrMult()
    {
        int currMult = baseMult + (incrementMult * level);
        return currMult;
    }
}
