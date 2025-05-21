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

    public PCard[] randomDraw(Deck deck, int drawCount)
    {
        PCard card = new PCard();
        PCard[] list = { card };
        return list;
    }

    public PCard[] randomCard (int cardCount)
    {
        PCard card = new PCard();
        PCard[] list = { card };
        return list;
    }

    public Voucher[] randomVoucher (int voucherCount)
    {
        Voucher card = new Voucher();
        Voucher[] list = { card };
        return list;
    }

    public Textbook[] randomTextbook (int textbookCount)
    {
        Textbook card = new Textbook();
        Textbook[] list = { card };
        return list;
    }

    public Mentor[] randomMentor (int mentorCount)
    {
        Mentor card = new Mentor();
        Mentor[] list = { card };
        return list;
    }

    public CardBuff[] randomCardBuff (int cardCount)
    {
        CardBuff card = new CardBuff();
        CardBuff[] list = { card };
        return list;
    }

    public Pack[] randomPack (int packCount)
    {
        Pack card = new Pack();
        Pack[] list = { card };
        return list;
    }

    public SpecialBlind randomSpecialBlind()
    {
        SpecialBlind blind = new SpecialBlind();
        return blind;
    }
      
}
