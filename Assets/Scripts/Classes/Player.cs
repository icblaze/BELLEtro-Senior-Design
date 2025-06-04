// This Document contains the code for the Player class
// This class contains information about what the player has
// Current Devs:
// Robert (momomonkeyman): made class and variables

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

    public Deck deck;                                                   //Deck of the game 
    public List<Mentor> mentorDeck = new List<Mentor>();                //List of mentors the player currently has.
    public List<Consumable> consumableDeck = new List<Consumable>();    //List of consumables the player currently has.
    public Deck hand;                                                   //Players hand in the game.
    public int discards;                                                //The amount of discards a player has in the game.                       
    public int handCount;                                               //The amount of hands the player can play in the game.
    public int moneyCount;                                              //Total money the player has in the game
    public BigInteger chipCount;                                        //Players chip count
    public int maxConsumables;                                          //Max consumables a player can have.
    public Voucher[] vouchers;                                          //Vouchers that the player has currently.
    public Dictionary<TextbookName, HandInfo> handTable;                //Dictionary that gets the handInfo from the poker hand played 
    public Game game;                                                   //Game object
    float discount;                                                     //Discount variable
    int sellValue;                                                      // This variable contains the sellValue of a card 

    //This removes the card from the players hand
    public void removeCard(PCard card)
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
