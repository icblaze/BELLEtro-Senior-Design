// This interface is the base for all of the mentor cards which will be 
// contained in the CardBuff directory which is located in this directory
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakid): created buff functions

//  6/10/25 (commented randomGenerating methods and going to move to different classes)

using System;
using System.Collections.Generic;
using UnityEngine;

// CardBuffs will change the cards that are in your hand, add consumables, and give money
public class CardBuff : Consumable
{
    public CardBuffName name;
    public ConsumableCardHolder consumableHolder;

    //  This will construct the appropiate CardBuff subclass
    public static CardBuff CardBuffFactory(CardBuffName name)
    {
        switch(name)
        {
            //  Generates the last used consumable
            case CardBuffName.Leftovers:
                return new Leftovers();
                
            //  TODO Add "Retake" seal to 1 card
            case CardBuffName.Coffee:
                return new Coffee();

            //  Generates up to 2 random Textbooks
            case CardBuffName.Almonds:
                return new Almonds();

            //  TODO Enhances up to 2 selected cards to "Mult" cards
            case CardBuffName.Cookies:
                return new Cookies();

            //  Generates up to 2 random Card Buffs
            case CardBuffName.Pancakes:
                return new Pancakes();

            //  TODO Enhances up to 2 selected cards to "Bonus" cards
            case CardBuffName.Chips:
                return new Chips();

            //  TODO Enhanced 1 card to "Wild" Card
            case CardBuffName.Popcorn:
                return new Popcorn();

            //  TODO Enhances 1 card to "Steel" Card
            case CardBuffName.Spinach:
                return new Spinach();

            //  TODO Enhances 1 card to "Glass" Card
            case CardBuffName.ChiliPepper:
                return new ChiliPepper();

            //  Doubles current money (up to $20)
            case CardBuffName.Egg:
                return new Egg();

            //  25% chance to add an Edition to random Mentor
            case CardBuffName.MysteryFood:
                return new MysteryFood();

            //  TODO Add "Study" seal to 1 card
            case CardBuffName.Tea:
                return new  Tea();

            //  TODO Destroys up to 2 selected cards
            case CardBuffName.IceCream:
                return new IceCream();

            //  TODO Select 2 cards, converts the left card into right
            case CardBuffName.Cherry:
                return new Cherry();

            //  Gives total sell value of current mentors
            case CardBuffName.Milk:
                return new Milk();

            //  TODO Enhances 1 card to "Gold" Card
            case CardBuffName.Cheese:
                return new Cheese();

            //  TODO Add "Funding" seal to 1 card
            case CardBuffName.Potato:
                return new Potato();

            //  TODO Increase Mult by 1 for up to 3 selected cards
            case CardBuffName.Flatbread:
                return new Flatbread();

            //  TODO Add a copy of 1 selected card to your deck
            case CardBuffName.Bagel:
                return new Bagel();

            //  Generates Textbook for most played Textbook hand
            case CardBuffName.Pretzel:
                return new Pretzel();

            //  Generate a random Mentor (must have room)
            case CardBuffName.Banana:
                return new Banana();

            //  TODO Increase chips by 10 for up to 3 selected cards
            case CardBuffName.Toast:
                return new Toast();

            default:
                return new Leftovers();
        }
    }

    Game game = Game.access();
    Player player = Player.access();

    //  Placeholder constructor (Leftovers)
    public CardBuff()
    {
        name = CardBuffName.Leftovers;
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
        type = ConsumableType.CardBuff;
        isDisabled = false;
        isNegative = false;
    }

    //  Set if the card buff can be used to set isDisabled, and return details
    public virtual string GetDescription()
    {
        return description;
    }

    //  Set if the card buff can be used to set isDisabled and return it
    public virtual bool CheckDisabled()
    {
        return isDisabled;
    }

    //  Will apply the card's specific buff, overridden in subclasses
    public virtual void applyCardBuff ()
    {

    }

    protected void GenerateConsumables(ConsumableType consumableType)
    {
        if(consumableHolder == null)
        {
            consumableHolder = GameObject.FindFirstObjectByType<ConsumableCardHolder>();
        }
        
        int space = player.maxConsumables - player.consumables.Count;

        //  Check if in player's consumables, free slot made
        foreach (CardBuff cardbuff in player.consumables)
        {
            if (cardbuff.name == name)
            {
                space++;
            }
        }

        //  Add random consumables 
        if (consumableType == ConsumableType.Textbook)
        {
            Textbook[] tbookList = game.randomTextbookShop(Math.Min(space, 2));
            consumableHolder.AddConsumable(tbookList[0]);
            consumableHolder.AddConsumable(tbookList[1]);
        }
        else
        {
            Consumable[] consumableList = game.randomCardBuffShop(Math.Min(space, 2));
            consumableHolder.AddConsumable(consumableList[0]);
            consumableHolder.AddConsumable(consumableList[1]);
        }
    }
}

