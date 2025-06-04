// This Document contains the code for the CardBuff class
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakid): made dict for instantUse, created buff functions

using System.Collections.Generic;
using UnityEngine;

// CardBuffs will change the cards that are in your hand, add consumables, and give money
public class CardBuff : Consumable
{
    public CardBuffName name;
    private static readonly Dictionary<CardBuffName, bool> instantDict = new()
    {
        { CardBuffName.Leftovers, true },
        { CardBuffName.Coffee, false },
        { CardBuffName.Almonds, true },
        { CardBuffName.Cookies, false},
        { CardBuffName.Pancakes, true},
        { CardBuffName.Chips, false},
        { CardBuffName.Popcorn, false},
        { CardBuffName.Spinach, false},
        { CardBuffName.ChiliPepper, false},
        { CardBuffName.Egg, true},
        { CardBuffName.MysteryFood, true},
        { CardBuffName.Tea, false},
        { CardBuffName.IceCream, false},
        { CardBuffName.Cherry, false},
        { CardBuffName.Milk, true},
        { CardBuffName.Cheese, false},
        { CardBuffName.Potato, false},
        { CardBuffName.Flatbread, false},
        { CardBuffName.Bagel, false},
        { CardBuffName.Pretzel, false},
        { CardBuffName.Banana, true},
        { CardBuffName.Toast, false}
    };

    //  Placeholder constructor (Leftovers)
    public CardBuff()
    {
        this.name = CardBuffName.Leftovers;
        price = 3;
        sellValue = 1;
        isInstant = true;
        type = ConsumableType.CardBuff;
        isDisabled = false;
        isNegative = false;
    }

    //  Construct CardBuff consumable, assigned isInstant based on name
    public CardBuff(CardBuffName name)
    {
        this.name = name;
        price = 3;
        sellValue = 1;
        isInstant = instantDict[name];
        type = ConsumableType.CardBuff;
        isDisabled = false;
        isNegative = false;
    }

    //  Will apply appropiate buff based on 
    public void applyCardBuff ()
    {
        
    }
}
