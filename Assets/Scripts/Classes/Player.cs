// This Document contains the code for the Player class
// This class contains information about what the player has
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakkid): turned arrays into List and added maxMentors variable;
// Zack: Added comments to the functions and variables for future developers.

using System;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using System.Linq;

// The Player class will contain the information about each of the things the
// player has and deals with selling
public class Player
{
    private static Player instance;  //Player instance varaiable

    //Singleton for the player
    public static Player access()
    {
        if (instance == null)
        {
            instance = new Player();
        }

        return instance;
    }

    public List<Mentor> mentorDeck;                              //List of Mentors that the player currently has.
    public List<Consumable> consumables;                         //List of consumables that the player currently has. Consumables are also called Textbooks.
    public int discards;                                         //The amount of discards the player currently has.
    public int handCount;                                        //The amoung of hands the player can play.
    public int maxDiscards;                                      //The max amount of discards of the player has, typically changed before round starts
    public int maxHandCount;                                     //The max hands the player can play, typically changed before round starts
    public int moneyCount;                                       //Money count for player.
    public BigInteger chipCount;                                 //Current chip count for the player.
    public int maxConsumables;
    public int maxMentors;
    public List<Voucher> vouchers;                               //List of vouchers that the player has.
    public Dictionary<TextbookName, HandInfo> handTable = new(); //This table contains the table that contains all the info for each textbook card.
    public float discount;                                       //Discount that can be used in store.
    public int sellValue;                                        //Sell value of a card enhancer
    public int handSize;
    public int maxInterest;
    public int handsPlayed;
    public int discardsPlayed;


    //Save player state in the game
    public void SaveState()
    {
        PlayerPrefsManager.SetMoney(moneyCount);
        PlayerPrefsManager.SetHandCount(handCount);
        PlayerPrefsManager.SetDiscardCount(discards);
        PlayerPrefsManager.SetRound(Game.roundValue);
        PlayerPrefsManager.SetAnte(Game.ante);
    }

    //Load players saved data to the game
    public void LoadState()
    {
        moneyCount = PlayerPrefsManager.GetMoney();
        Game.roundValue = PlayerPrefsManager.GetRound();
        handCount = PlayerPrefsManager.GetHandCount();                 
        discards = PlayerPrefsManager.GetDiscardCount();
        Game.ante = PlayerPrefsManager.GetAnte();
    }

    //  The player constructor
    private Player()
    {
        maxMentors = 5;
        maxConsumables = 2;
        mentorDeck = new List<Mentor>(maxMentors);
        consumables = new List<Consumable>(maxConsumables);
        vouchers = new List<Voucher>();
        discards = 4;
        handCount = 4;
        maxDiscards = 4;
        maxHandCount = 4;
        moneyCount = 4;
        chipCount = 0;
        discount = 1.0f;    //  100% price initially
        handSize = 8;
        maxInterest = 5;
        handsPlayed = 0;
        discardsPlayed = 0;
        InitializeHandTable();
    }

    //  Reset the player to intial values
    public void Reset()
    {
        maxMentors = 5;
        maxConsumables = 2;
        mentorDeck = new List<Mentor>(maxMentors);
        consumables = new List<Consumable>(maxConsumables);
        vouchers = new List<Voucher>();
        discards = 4;
        handCount = 4;
        maxDiscards = 4;
        maxHandCount = 4;
        moneyCount = 4;
        chipCount = 0;
        discount = 1.0f;    // 100% price initially
        sellValue = 0;
        handSize = 8;
        maxInterest = 5;
        handsPlayed = 0;
        discardsPlayed = 0;
        handTable.Clear();
        InitializeHandTable();
    }

    //This removes a card from the player hand list if it is contained within the list
    public void removeCard(PCard card)
    {
        Deck deck = Deck.access();

        if (deck.playerHand.Contains(card))
        {
            deck.playerHand.Remove(card);
        }
        else
        {
            Debug.Log("Card not found in players hand, unable to remove it.");
        }
    }

    //This function is responsible for selling the mentor, and updates the players money count.
    //This function also removes the mentor from the list that contains all the mentors for the player
    public void sellMentor(Mentor mentor)
    {
        sellValue = (int)math.max(1, math.floor(mentor.price / 2));
        Player currPlayer = access();
        currPlayer.moneyCount += sellValue;
        currPlayer.mentorDeck.Remove(mentor);
    }

    //This function is responsible for selling the consumable and updates the players money count.
    public void sellConsumable(Consumable consumable)
    {
        sellValue = (int)math.max(1, math.floor(consumable.price / 2));
        Player currPlayer = access();
        currPlayer.moneyCount += sellValue;
        currPlayer.consumables.Remove(consumable);
    }

    //  This function initializes the player's handTable, and this defines the data for a specific hand.
    public void InitializeHandTable()
    {
        foreach (TextbookName handName in Enum.GetValues(typeof(TextbookName)))
        {
            if (!handTable.ContainsKey(handName))
            {
                handTable.Add(handName, new HandInfo(handName));
            }
        }
    }

}