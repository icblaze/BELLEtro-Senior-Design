using System.Numerics;
using TMPro;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public GameObject victoryText;
    public static BigInteger bestHand = 0;
    public static int handsPlayed = 0;
    public static int cardsPurchased = 0;
    public static int packsPurchased = 0;
    public static int numRerolls = 0;
    private static VictoryManager instance;  //VictoryManager instance varaiable

    //Singleton for the VictoryManager
    public static VictoryManager access()
    {
        return instance;
    }

    // Enforce singleton instance
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }

        instance = this;
    }

    public void GameWon()
    {
        victoryText = GameObject.Find("Victory Text");
        string bestHandString = "Best Hand Played: " + bestHand.ToString() + "\n";
        string handsPlayedString = "Hands Played: " + handsPlayed.ToString() + "\n";
        string cardsPurchasedString = "Cards Purchased: " + cardsPurchased.ToString() + "\n";
        string packsPurchasedString = "Packs Purchased: " + packsPurchased.ToString() + "\n";
        string numRerollsString = "Number of Rerolls: " + numRerolls.ToString() + "\n";
        victoryText.GetComponent<TMP_Text>().text = "excELLEntly Job\n" + bestHandString + handsPlayedString + cardsPurchasedString + packsPurchasedString + numRerollsString;
        ResetGameVictory();
    }

    public void ResetGameVictory()
    {
        bestHand = 0;
        cardsPurchased = 0;
        packsPurchased = 0;
        numRerolls = 0;
    }
    //Setters
    public void SetBestHand(BigInteger i)
    {
        bestHand = i;
    }
    public void SetHandsPlayed(int i)
    {
        handsPlayed = i;
    }
    public void SetCardsPurchased(int i)
    {
        cardsPurchased = i;
    }
    public void SetPacksPurchased(int i)
    {
        packsPurchased = i;
    }
    public void SetNumRerolls(int i)
    {
        numRerolls = i;
    }
    //Getters
    public BigInteger GetBestHand()
    {
        return bestHand;
    }
    public int GetHandsPlayed()
    {
        return handsPlayed;
    }
    public int GetCardsPurchased()
    {
        return cardsPurchased;
    }
    public int GetPacksPurchased()
    {
        return packsPurchased;
    }
    public int GetNumRerolls()
    {
        return numRerolls;
    }
}
