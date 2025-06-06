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
    public CardType kindOfCard;
    //public int sellValue; //Removed sell value since we can calculate the sell value manually
    public ConsumableType type;
    public bool isInstant;
    public int price;
    public int sellValue;
    public bool isNegative;
    public bool isDisabled;
}
