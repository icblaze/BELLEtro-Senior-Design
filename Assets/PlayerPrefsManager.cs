//Script is used as a manager for the PlayerPrefs for the game.
//This includes the setters, the getters, and the intial values for the
//PlayerPrefs.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPrefsManager
{
    private const string MONEY = "money";
    private const string CHIPS = "chips";
    private const string ANTE = "ante";
    private const string ROUND = "round";
    private const string HandCount = "hands";
    private const string DiscardCount = "discards";

    public static void SetHandCount(int hand)
    {
        PlayerPrefs.SetInt(HandCount, hand);
    }
    public static int GetHandCount()
    {
        return PlayerPrefs.GetInt(HandCount);
    }
    public static bool HasHandCount()
    {
        return PlayerPrefs.HasKey("HandCount");
    }

    public static void SetDiscardCount(int discard)
    {
        PlayerPrefs.SetInt(DiscardCount, discard);
    }
    public static int GetDiscardCount()
    {
        return PlayerPrefs.GetInt(DiscardCount);
    }
    public static void SetMoney(int money)
    {
        PlayerPrefs.SetInt(MONEY, money);
    }
    public static int GetMoney()
    {
        return PlayerPrefs.GetInt(MONEY);
    }

    public static void SetChips(int chips)
    {
        PlayerPrefs.SetInt(CHIPS, chips);
    }
    private static int GetChips()
    {
        return PlayerPrefs.GetInt(CHIPS);
    }

    public static void SetAnte(int ante)
    {
        PlayerPrefs.SetInt(ANTE, ante);
    }
    public static int GetAnte()
    {
        return PlayerPrefs.GetInt(ANTE);
    }
    public static void SetRound(int round)
    {
        PlayerPrefs.SetInt(ROUND, round);
    }
    public static int GetRound()
    {
        return PlayerPrefs.GetInt(ROUND);
    }
    //Clear all player prefs and set everything to default values.
    public static void ClearAll()
    {
        PlayerPrefs.SetInt(MONEY, 0);
        PlayerPrefs.SetInt(CHIPS, 0);
        PlayerPrefs.SetInt(ANTE, 1);
        PlayerPrefs.SetInt(ROUND, 1);
        PlayerPrefs.SetInt(HandCount, 4);
        PlayerPrefs.SetInt(DiscardCount, 4);
        PlayerPrefs.Save();
    }
}

