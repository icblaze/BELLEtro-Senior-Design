// This Document contains the code for the Textbook class
// This class contains information about a Textbook which is a type of consumable
// Current Devs:
// Robert (momomonkeyman): made class and varuables

using System.Collections;
using UnityEngine;

// Textbooks will enhance the base chips and mult for a kind of hand
public class Textbook : Consumable
{
    public CardType kindOfCard;

  public:
    TextbookName name;
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
