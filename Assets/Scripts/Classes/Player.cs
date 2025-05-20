// This Document contains the code for the Player class
// This class contains information about what the player has
// Current Devs:
// Robert (momomonkeyman): made class and variables
//
using System.Numerics;
using System.Collections;
using UnityEngine;

// The Player class will contain the information about each of the things the
// player has and deals with selling
public class Player
{
  public:
    Deck deck;
    Mentor[] mentorDeck;
    Consumable[] consumables;
    Deck hand;
    int discards;
    int handCount;
    int moneyCount;
    BigInteger chipCount;
    int maxConsumables;
    Voucher[] vouchers;
    // Make Hand Table here  <--------
    Game game;
    float discount;
    
    public void removeCard (Card card)
    {

    }

    public void sellMentor (Mentor mentor)
    {

    }

    public void sellConsumable (Consumable consumable)
    {

    }
}
