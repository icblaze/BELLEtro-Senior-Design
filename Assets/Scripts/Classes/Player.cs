// This Document contains the code for the Player class
// This class contains information about what the player has
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakkid): turned arrays into List and added maxMentors variable;
// Zack: Added comments to the functions and variables for future developers.

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
    public List<Mentor> mentorDeck;                              //List of Mentors that the player currently has.
    public List<Consumable> consumables;                         //List of consumables that the player currently has. Consumables are also called Textbooks.
    public int discards;                                         //The amount of discards the player currently has.
    public int handCount;                                        //The amoung of hands the player can play.
    public int moneyCount;                                       //Money count for player.
    public BigInteger chipCount;                                 //Current chip count for the player.
    public int maxConsumables;                                   
    public int maxMentors;              
    public List<Voucher> vouchers;                               //List of vouchers that the player has.
    public Dictionary<TextbookName, HandInfo> handTable;         //This table conatins the table that contains all the info for each textbook card.
    float discount;                                              //Discount that can be used in store.
    public int sellValue;                                        //Sell value of a card enhancer
    
    //This removes a card from the player hand list if it is contained within the list
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
}
