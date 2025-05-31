using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Script is used as a manager for the PlayerPrefs for the game.
//This includes the setters, the getters, and the intial values for the
//PlayerPrefs.
public class PlayerPrefsManager
{
    private const string MONEY = "money";
    private const string CHIPS = "chips";
    private const string ANTE = "ante";
    private const string ROUND = "round";

    public static void SetMoney(int money)
    {
        PlayerPrefs.SetInt(MONEY, money);
    }
    public static int GetMoney()
    {
        return PlayerPrefs.GetInt(CHIPS);
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
        PlayerPrefs.Save();
    }
}
