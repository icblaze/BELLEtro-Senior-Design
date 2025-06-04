// This Document contains the code for the Player class
// This class contains information about what the player has
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakkid): turned arrays into List and added maxMentors variable;

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

    public Deck deck;
    public List<Mentor> mentorDeck;
    public List<Consumable> consumables;
    public Deck hand;
    public int discards;
    public int handCount;
    public int moneyCount;
    public BigInteger chipCount;
    public int maxConsumables;
    public int maxMentors;
    public List<Voucher> vouchers;
    public Dictionary<TextbookName, HandInfo> handTable;
    public Game game;
    float discount;
    
    public void removeCard (PCard card)
    {
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
        currPlayer.consumableDeck.Remove(consumable);
    }
}
