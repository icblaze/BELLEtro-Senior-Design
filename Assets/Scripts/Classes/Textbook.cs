// This Document contains the code for the Textbook class
// This class contains information about a Textbook which is a type of consumable
// Current Devs:
// Robert (momomonkeyman): made class and varuables

using System.Collections;
using UnityEngine;

// Textbooks will enhance the base chips and mult for a kind of hand
public class Textbook : Consumable
{
    public TextbookName name;

    //  Placeholder default constructor (High Card)
    public Textbook()
    {
        name = TextbookName.HighCard;
        price = 3;
        sellValue = 1;
        isInstant = true;
        type = ConsumableType.Textbook;
        isDisabled = false;
        isNegative = false;
    }

    //  Construct Textbook consumable with name of hand
    public Textbook(TextbookName name)
    {
        this.name = name;
        price = 3; 
        sellValue = 1; 
        isInstant = true; 
        type = ConsumableType.Textbook;
        isDisabled = false;
        isNegative = false;
    }

    //  Increases appropiate hand based on textbook name
    public void applyTextbook (Game game)
    {
        game.ThePlayer.handTable[name].increaseLevel();
    }
}
