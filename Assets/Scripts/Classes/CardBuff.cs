// This Document contains the code for the CardBuff class
// This class contains information about a CardBuff which is a type of consumable
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakid): made dict for instantUse, created buff functions

using System;
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

    Game game = Game.access();
    Player player = Player.access();

    //  Placeholder constructor (Leftovers)
    public CardBuff()
    {
        name = CardBuffName.Leftovers;
        price = 3;
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
        isInstant = instantDict[name];
        type = ConsumableType.CardBuff;
        isDisabled = false;
        isNegative = false;
    }

    //  Check if the card buff can be used to set isDisabled, and return details
    public virtual string CheckAvailability()
    {
        return "test";
    }

    //  Will apply appropiate buff based on name. 
    public virtual void applyCardBuff ()
    {
        //  Use effect of card buff
        switch(name)
        {
            //  Generates the last used consumable
            case CardBuffName.Leftovers:
                Consumable lastUsed = game.previousConsumable;
                if (lastUsed.type == ConsumableType.Textbook)
                {
                    Textbook prevTextbook = (Textbook) game.previousConsumable;
                    player.consumables.Add(new Textbook(prevTextbook.name));
                }
                else if (lastUsed.type == ConsumableType.CardBuff)
                {
                    CardBuff prevCardBuff = (CardBuff) game.previousConsumable;
                    player.consumables.Add(new CardBuff(prevCardBuff.name));
                }
                break;

            //  TODO Add "Retake" seal to 1 card
            case CardBuffName.Coffee:
                break;

            //  Generates up to 2 random Textbooks
            case CardBuffName.Almonds:
                GenerateConsumables(ConsumableType.Textbook);
                break;

            //  TODO Enhances up to 2 selected cards to "Mult" cards
            case CardBuffName.Cookies:
                break;

            //  Generates up to 2 random Card Buffs
            case CardBuffName.Pancakes:
                GenerateConsumables(ConsumableType.CardBuff);
                break;

            //  TODO Enhances up to 2 selected cards to "Bonus" cards
            case CardBuffName.Chips:
                break;

            //  TODO Enhanced 1 card to "Wild" Card
            case CardBuffName.Popcorn:
                break;

            //  TODO Enhances 1 card to "Steel" Card
            case CardBuffName.Spinach:
                break;

            //  TODO Enhances 1 card to "Glass" Card
            case CardBuffName.ChiliPepper:
                break;

            //  Doubles current money (up to $20)
            case CardBuffName.Egg:
                player.moneyCount += Math.Min(player.moneyCount * 2, 20);
                break;

            //  25% chance to add an Edition to random Mentor
            case CardBuffName.MysteryFood:
                System.Random rand = new System.Random();

                List<Mentor> baseMentors = new List<Mentor>();

                foreach(Mentor mentor in player.mentorDeck)
                {
                    if(mentor.edition == CardEdition.Base)
                    {
                        baseMentors.Add(mentor);
                    }
                }

                if(rand.NextDouble() < 0.25)
                {
                    int mentorIndex = rand.Next(baseMentors.Count);

                    // +1 because can't be base; also no weighing here
                    CardEdition randEdition = (CardEdition) 1 + rand.Next(Enum.GetValues(typeof(CardEdition)).Length);
                    player.mentorDeck[mentorIndex].edition = randEdition;
                }
                break;

            //  TODO Add "Study" seal to 1 card
            case CardBuffName.Tea:
                break;

            //  TODO Destroys up to 2 selected cards
            case CardBuffName.IceCream:
                break;

            //  TODO Select 2 cards, converts the left card into right
            case CardBuffName.Cherry:
                break;

            //  Gives total sell value of current mentors
            case CardBuffName.Milk:
                int totalSell = 0;
                foreach (Mentor mentor in player.mentorDeck)
                {
                    totalSell += mentor.sellValue;
                }
                player.moneyCount += totalSell;
                break;

            //  TODO Enhances 1 card to "Gold" Card
            case CardBuffName.Cheese:
                break;

            //  TODO Add "Funding" seal to 1 card
            case CardBuffName.Potato:
                break;

            //  TODO Converts up to 3 selected cards into random Voiceless cards
            case CardBuffName.Flatbread:
                break;

            //  TODO Converts up to 3 selected cards into random Voiced cards
            case CardBuffName.Bagel:
                break;

            //  TODO Converts up to 3 selected cards into random Lax cards
            case CardBuffName.Pretzel:
                break;

            //  Generate a random Mentor (must have room)
            case CardBuffName.Banana:
                if (player.mentorDeck.Count < player.maxMentors)
                {
                    player.mentorDeck.AddRange(game.randomMentor(1));
                }
                else
                {
                    isDisabled = true;
                }
                break;

            //  TODO Converts up to 3 selected cards into random Tense cards
            case CardBuffName.Toast:
                break;
        }

        //  Set prev used consumable to current textbook (except for Leftovers)
        if (name != CardBuffName.Leftovers)
        {
            game.previousConsumable = new CardBuff(name);
        }
    }

    //  Generates up to 2 consuables of given type
    private void GenerateConsumables(ConsumableType consumableType)
    {
        
        int space = player.maxConsumables - player.consumables.Count;

        //  Check if in player's consumables, free slot made
        foreach (CardBuff cardbuff in player.consumables)
        {
            if (cardbuff.name == name)
            {
                space++;
            }
        }

        //  Add random consumables (change this later/move to different classes)
        if (consumableType == ConsumableType.Textbook)
        {
            //player.consumables.AddRange(game.randomTextbook(Math.Min(space, 2)));
        }
        else
        {
            //player.consumables.AddRange(game.randomCardBuff(Math.Min(space, 2)));
        }
    }
}
