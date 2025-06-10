// This Document contains the code for the Consumable interface
// This Interface will branch into the Textbook and CardBuff classes
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// This interface extends the BaseCard interface and has basic information for 
// each type of consumable

public abstract class Consumable : BaseCard
{
    private CardType _kindOfCard;
    public ConsumableType type;
    public bool isInstant;
    public int price;
    public int sellValue;
    public bool isNegative;
    public bool isDisabled;         // This variable is used to indicate if a consumable is able to be used
    
    //Getter and setter for setting up the property _kindOfCard
    public override CardType kindOfCard
    {
        get { return _kindOfCard; }
        set { kindOfCard = value; }
    } 

}
