// This Document contains the code for the Consumable interface
// This Interface will branch into the Textbook and CardBuff classes
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// This interface extends the BaseCard interface and has basic information for 
// each type of consumable
}
public interface Consumable : BaseCard
{
    public CardType kindOfCard;
  public:
    int sellValue;
    ConsumableType type;
    bool isInstant;
    int price;
    bool isNegative;
    bool isDisabled;

    public void applyEffect(Game game)
    {

    }
}
