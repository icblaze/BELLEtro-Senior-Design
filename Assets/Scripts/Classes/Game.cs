// This Document contains the code for the Game class.
// This class is used to hold information about the game's state
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Numerics;
using System.Collections;
using UnityEngine;

// the Game class contains all of the information about the Game
public class Game
{
    public int Ante;        // Ante is the set of Rounds the player is on
    public int RoundValue;  // RoundValue is the Round within the current Ante
    public BigInteger BaseChips;   // BaseChips are a calculation point for the value in each round
    public SpecialBlind[] PastSpecialBlinds; // PastSpecialBlinds are the used Blinds 
    public int ChipTotal;   // ChipTotal is the number of chips needed to win a round
    public Voucher[] VoucherHolder; // VoucherHolder contains the current Ante's Vouchers
    public SpecialBlind CurrentSpecialBlind; // CurrentSpecialBlind contains this Ante's Special Blind
    public Player ThePlayer; // ThePlayer is a refrence to the Player class 

    void randomizer ()
    {

    }

    public Card[] randomDraw (Deck deck, int drawCount)
    {

    }

    public Card[] randomCard (int cardCount)
    {

    }

    public Voucher[] randomVoucher (int voucherCount)
    {

    }

    public Textbook[] randomTextbook (int textbookCount)
    {

    }

    public Mentor[] randomMentor (int mentorCount)
    {

    }

    public CardBuff[] randomCardBuff (int cardCount)
    {

    }

    public Pack[] randomPack (int packCount)
    {

    }

    public SpecialBlind randomSpecialBlind()
    {

    }
      
}
