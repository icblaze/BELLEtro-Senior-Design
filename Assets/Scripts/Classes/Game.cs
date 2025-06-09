// This Document contains the code for the Game class.
// This class is used to hold information about the game's state
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Andy (flakkid): added previous consumable variable

using System.Numerics;
using System.Collections;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;



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

    private int Ante;                            //Ante is the set of Rounds the player is on
    private int RoundValue;                      //RoundValue is the Round within the current Ante
    public BigInteger BaseChips;                 //BaseChips are a calculation point for the value in each round, these are the blue chips.
    public SpecialBlind[] PastSpecialBlinds;     //PastSpecialBlinds are the used Blinds 
    private int ChipTotal;                       //ChipTotal is the number of chips needed to win a round
    public Voucher[] VoucherHolder;              //VoucherHolder contains the current Ante's Vouchers
    public SpecialBlind CurrentSpecialBlind;     //CurrentSpecialBlind contains this Ante's Special Blind
    public Player ThePlayer;                     //The Player is a refrence to the Player class 
    private int index;                           //This variable will hold a value that we would use to index into our deck
    private static Random rnd = new Random();    //Random number generator

    public Consumable previousConsumable;        //  Stores name of last used consumable


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
        Player player = Player.access();
        
        List<PCard> card = player.deck.deckCards;

        PCard card = new PCard();
        PCard[] list = { card };
        return list;
    }

    //This will generate a random voucher for the shop.
    public Voucher[] randomVoucher(int voucherCount)
    {
        Voucher card = new Voucher();
        Voucher[] list = { card };
        return list;
    }

    //This will generate a random textbook card for the shop.
    public List<CardObject> randomTextbook(Pack pack)
    {
        Textbook card = new Textbook();
        Textbook[] list = { card };
        return list;
    }

    //This function is responsible for retrieving random Mentors for the shop.
    public Mentor[] randomMentor(Pack pack)
    {
        Mentor card = new Mentor();
        Mentor[] list = { card };
        return list;
    }

    //This function is responsible for retrieving random card buffs for the shop.
    public List<CardObject> randomCardBuff(Pack pack)
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
        SpecialBlind blind = new SpecialBlind();
        return blind;
    }
      
}

