// This Document contains the code for the Game class.
// This class is used to hold information about the game's state
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Numerics;
using System.Collections;
using System;



// The Game class contains all of the information about the Game
public class Game
{
    
    private static Game instance;
    
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
    private int ChipTotal;                       // ChipTotal is the number of chips needed to win a round
    public Voucher[] VoucherHolder;              //VoucherHolder contains the current Ante's Vouchers
    public SpecialBlind CurrentSpecialBlind;     //CurrentSpecialBlind contains this Ante's Special Blind
    public Player ThePlayer;                     //ThePlayer is a refrence to the Player class 
    private int index;                           //This varaibale will hold a value that we would use to index into our deck
    private static Random rnd = new Random();    //Random number generator


    //This function is used to create a seed, so that we can get a random card from the deck.
    public int randomizer(int deckSize)
    {
        return rnd.Next(0, deckSize);
    }

    //This will randomly draw cards for the deck after a player plays a hand or removes cards from their deck.
    public PCard[] randomDraw(Deck deck, int drawCount)
    {
        PCard[] list = new PCard[drawCount];
        PCard card;

        for (int i = 0; i < drawCount; i++)
        {
            //Call the randomizer function so that we can get a random number so that we can get a random card from the deck.
            index = randomizer(Deck.deckSize);

            //Next we should index into our deck so that we can get a card from our deck.'
            
            //Then we should add that card to the list of cards.
            card = new PCard();

        }
        
        return list;
    }

    //This function is responsible for generating a random card, useful for when we want to retrieve a random card for packs.
    public PCard[] randomCard(int cardCount)
    {
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
    public Textbook[] randomTextbook(int textbookCount)
    {
        Textbook card = new Textbook();
        Textbook[] list = { card };
        return list;
    }

    //This function is responsible for retrieving random Mentors for the shop.
    public Mentor[] randomMentor(int mentorCount)
    {
        Mentor card = new Mentor();
        Mentor[] list = { card };
        return list;
    }

    //This function is responsible for retrieving random card buffs for the shop.
    public CardBuff[] randomCardBuff(int cardCount)
    {
        CardBuff card = new CardBuff();
        CardBuff[] list = { card };
        return list;
    }

    //This is function is responsible for retrieving random packs for the shop.
    public Pack[] randomPack(int packCount)
    {
        Pack card = new Pack();
        Pack[] list = { card };
        return list;
    }

    //This function is responsible for retrieving random special blinds for the Ante
    public SpecialBlind randomSpecialBlind()
    {
        SpecialBlind blind = new SpecialBlind();
        return blind;
    }
      
}
