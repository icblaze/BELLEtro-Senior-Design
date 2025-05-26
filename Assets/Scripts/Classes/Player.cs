// This Document contains the code for the Player class
// This class contains information about what the player has
// Current Devs:
// Robert (momomonkeyman): made class and variables

using System.Numerics;
using System.Collections;
using UnityEngine;

// The Player class will contain the information about each of the things the
// player has and deals with selling
public class Player
{
    public Deck deck;
    public Mentor[] mentorDeck;
    public Consumable[] consumables;
    public Deck hand;
    public int discards;
    public int handCount;
    public int moneyCount;
    public BigInteger chipCount;
    public int maxConsumables;
    public Voucher[] vouchers;
    // Make Hand Table here  <--------
    public Game game;
    float discount;
    
    public void removeCard (PCard card)
    {

    }

    public void sellMentor (Mentor mentor)
    {

    }

    public void sellConsumable (Consumable consumable)
    {

    }
}
