// This Document contains the code for the CardBuff class
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Robert (momomonkeyman): made class and varuables

using System.Collections;
using UnityEngine;

// CardBuffs will change the cards that are in your hand, add consumables, and give money
public class CardBuff : Consumable
{
    public CardType kindOfCard;

  public:
    CardBuffName name;
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
