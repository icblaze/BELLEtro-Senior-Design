// This Document contains the code for the Game class.
// This class is used to hold information about the game's state
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Andy (flakkid): added previous consumable variable
// Zacharia Alaoui (ZachariaAlaoui): Added the functions and the logic for the functions
// Fredrick (bouloutef04): Added functions to obtain mentors, cardbuffs, and textbooks for the shop.

using System.Numerics;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using System.Xml;
using Unity.Collections;



// The Game class contains all of the information about the Game
public class Game
{

    private static Game instance;

    //Singelton for the Game class
    public static Game access()
    {
        if (instance == null)
        {
            instance = new Game();
        }
        return instance;
    }

    private int Ante;                                   //Ante is the set of Rounds the player is on
    private int RoundValue;                             //RoundValue is the Round within the current Ante
    public BigInteger BaseChips;                        //BaseChips are a calculation point for the value in each round, these are the blue chips.
    public SpecialBlind[] PastSpecialBlinds;            //PastSpecialBlinds are the used Blinds 
    private int ChipTotal;                              //ChipTotal is the number of chips needed to win a round
    public Voucher[] VoucherHolder;                     //VoucherHolder contains the current Ante's Vouchers
    public SpecialBlind CurrentSpecialBlind;            //CurrentSpecialBlind contains this Ante's Special Blind
    public Player ThePlayer;                            //The Player is a refrence to the Player class 
    private int index;                                  //This variable will hold a value that we would use to index into our deck
    private static Random rnd = new Random();           //Random number generator
    public Consumable previousConsumable = null;        //  Stores name of last used consumable


    //This function is used to create a seed, so that we can get a random card from the deck.
    //This randomizer will also be used to retrieve a random index so we can select a random consumable/pack.
    public int randomizer(int Size)
    {
        return rnd.Next(0, Size);
    }

    //This will randomly draw cards from the deck after a player plays a hand or removes cards from the deck.
    public PCard[] randomDraw(List<PCard> deckCards, int drawCount)
    {
        PCard[] list = new PCard[drawCount];

        for (int i = 0; i < drawCount; i++)
        {
            //Call the randomizer function so that we can get a random number so that we can get a random card from the deck.
            index = randomizer(deckCards.Count);

            //Next we should index into our deck so that we can get a card from our deck.
            list[i] = deckCards[index];
        }

        return list;
    }

    //This function is responsible for generating a random card, useful for when we want to retrieve a random card for packs.
    public List<CardObject> randomPackCards(Pack pack)
    {
        //Get access to the Deck 
        Deck deck = Deck.access();

        //This list of card objects will contain the random cards that were selected from the deck
        List<CardObject> cards = new List<CardObject>();

        //Retrieve random cards from the deck so that we can potentially apply card modifiers to the cards
        for (int i = 0; i < pack.packSize; i++)
        {
            int index = randomizer(deck.deckCards.Count);

            CardObject newCard = new CardObject();
            newCard.cardData = deck.deckCards[index];


            //Call a function here to determine what the enhancements will be on the current card
            applyEnhancement(newCard, pack.edition);

            cards.Add(newCard);
        }

        return cards;
    }

    //This function modifies a deck card , and applies any modifiers to it so that it can be added to a pack for use.
    public CardObject applyEnhancement(CardObject newCard, PackEdition packEdition)
    {
        //Set the temporary dictionaries with the ones in the CardModifier class.
        //This allows us to enhance the dictionaries below based on packtype, this allows rarer cards show up in higher ranked packs.
        var adjustedEditions = CardModifier.editionRates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var adjustedEnhancements = CardModifier.enhancementRates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        var adjustedSeals = CardModifier.sealRates.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        if (packEdition == PackEdition.Jumbo_Pack)
        {
            //This updates all the keys in the dictionary that are less than 60.
            //This piece of code doubles the Key in the dictionary, which gives 
            //rarer cards a higher chance to be selected

            adjustedEditions = adjustedEditions.ToDictionary(
                kvp => (kvp.Key < 60) ? kvp.Key * 2 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedEnhancements = adjustedEnhancements.ToDictionary(
                kvp => (kvp.Key < 40) ? kvp.Key * 2 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedSeals = adjustedSeals.ToDictionary(
                kvp => (kvp.Key < 50) ? kvp.Key * 2 : kvp.Key,
                kvp => kvp.Value
            );

        }
        else if (packEdition == PackEdition.Mega_Pack)
        {
            adjustedEditions = adjustedEditions.ToDictionary(
                kvp => (kvp.Key < 60) ? kvp.Key * 3 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedEnhancements = adjustedEnhancements.ToDictionary(
                kvp => (kvp.Key < 40) ? kvp.Key * 3 : kvp.Key,
                kvp => kvp.Value
            );

            adjustedSeals = adjustedSeals.ToDictionary(
                kvp => (kvp.Key < 50) ? kvp.Key * 3 : kvp.Key,
                kvp => kvp.Value
            );
        }

        newCard.cardData.seal = CardModifier.GetWeightedModifier(adjustedSeals);
        newCard.cardData.enhancement = CardModifier.GetWeightedModifier(adjustedEnhancements);
        newCard.cardData.edition = CardModifier.GetWeightedModifier(adjustedEditions);


        return newCard;
    }

    //This will generate a random voucher for the shop.
    public Voucher[] randomVoucher(int voucherCount)
    {
        Voucher[] vouchers = new Voucher[voucherCount];
        int count = 0;

        while (count < voucherCount)
        {
            index = randomizer(Enum.GetValues(typeof(VoucherNames)).Length);    // Select a random number that is in the range of the TextbookName enum 
            Voucher voucherCard = new Voucher();                                // Created a new voucher card  

            bool alreadyExists = Array.Exists(vouchers, voucher => voucher == voucherCard);


            if (!alreadyExists)
            {
                vouchers[count] = voucherCard;
                count++;
            }
            else
            {
                continue;
            }

        }

        return vouchers;
    }

    //This will generate a random textbook card for packs.
    public List<CardObject> randomTextbook(Pack pack)
    {
        Textbook card = new Textbook();
        Textbook[] list = { card };
        List<CardObject> list2 = new List<CardObject>();
        return list2;
    }

    //This will generate a random textbook card for the shop.
    public Textbook[] randomTextbookShop(int num)
    {
        Textbook[] list = new Textbook[num];
        int count = 0;

        while (count < num)
        {
            index = randomizer(Enum.GetValues(typeof(TextbookName)).Length);    // Select a random number that is in the range of the TextbookName enum 
            Textbook textBookCard = new Textbook((TextbookName)index);          // Created a new textbook card  

            bool alreadyExists = Array.Exists(list, textBook => textBook == textBookCard);

            //This checks if the textBook already exists within the array.
            //This statement helps prevent producing duplicate textbook cards.
            if (!alreadyExists)
            {
                list[count] = textBookCard;
                count++;
            }
            else
            {
                continue;
            }

        }

        return list;
    }

    //This function is responsible for retrieving random Mentors for packs.
    public List<CardObject> randomMentor(Pack pack)
    {
        Mentor card = new Mentor();
        Mentor[] list = { card };
        List<CardObject> list2 = new List<CardObject>();
        return list2;
    }

    //This function is responsible for retrieving random Mentors for the shop.
    public Mentor[] randomMentorShop()
    {
        Mentor card = new Mentor();
        Mentor[] list = { card };
        return list;
    }

    //This function is responsible for retrieving random card buffs for packs.
    public List<CardObject> randomCardBuff(Pack pack)
    {
        List<CardObject> cardBuffCards = new List<CardObject>();    //This list contains card objects

        int count = 0;
        while (count < pack.packSize)
        {
            int index = randomizer(Enum.GetValues(typeof(CardBuffName)).Length);
            CardObject newCardBuff = new CardObject();

            newCardBuff.cardData.cardBuff = CardBuff.CardBuffFactory((CardBuffName)index);  //Create a CardBuff and assign it to our cardObject

            //Checks if the cardBuff already exists in the cardBuffCards list
            bool alreadyExists = cardBuffCards.Any(cardBuffCard => cardBuffCard.cardData.cardBuff.name == newCardBuff.cardData.cardBuff.name);

            if (!alreadyExists)
            {
                cardBuffCards.Add(newCardBuff);
                count++;
            }
            else
            {
                continue;
            }
        }

        return cardBuffCards;
    }
    //This function is responsible for retrieving random card buffs for the shop.
    public CardBuff[] randomCardBuffShop()
    {

        CardBuff card = new CardBuff();
        CardBuff[] list = { card };
        return list;
    }

    //This is function is responsible for retrieving random packs for the shop.
    public Pack[] randomPacks(int packCount)
    {
        Pack[] pack = new Pack[packCount];

        for (int i = 0; i < packCount; i++)
        {
            pack[i] = new Pack(); //Instantiate a new pack object

            PackEdition selectedEdition = (PackEdition)rnd.Next(Enum.GetValues(typeof(PackEdition)).Length); // Get a PackEdition value that corresponds to a packEdition in the enum list
            PackType selectedType = (PackType)rnd.Next(Enum.GetValues(typeof(PackType)).Length);             // Get the PackType value that corresponds to a PackType in the enum list

            //The following code fills all the variables for the pack
            pack[i].packType = selectedType;
            pack[i].edition = selectedEdition;
            pack[i].price = pack[i].getPackPrice(selectedEdition);
            pack[i].selectableCards = pack[i].getSelectableCount(selectedEdition);
            pack[i].packSize = pack[i].getPackSize(selectedEdition);

            if (pack[i].packType == PackType.Standard_Pack)
            {
                pack[i].cardsInPack = randomPackCards(pack[i]);
            }
            else if (pack[i].packType == PackType.CardBuff_Pack)
            {
                pack[i].cardsInPack = randomCardBuff(pack[i]);
            }
            else if (pack[i].packType == PackType.Textbook_Pack)
            {
                pack[i].cardsInPack = randomTextbook(pack[i]);
            }
            else if (pack[i].packType == PackType.Mentor_Pack)
            {
                pack[i].cardsInPack = randomMentor(pack[i]);
            }
        }

        return pack;
    }

    //This function is responsible for retrieving random special blinds for the Ante
    public SpecialBlind randomSpecialBlind()
    {
        index = randomizer(Enum.GetValues(typeof(SpecialBlind)).Length);

        SpecialBlind blind = new SpecialBlind((SpecialBlindNames)index); 

        //Add the blind to the past special blinds array

        return blind;
    }

    public BigInteger GetChipTotal()
    {
        return ChipTotal;
    }

}

