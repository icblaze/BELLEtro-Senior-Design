// This file will contain all the logic for the Round screen in belletro, and 
// this file will contain all the logic for expressing the effects during the round
// on the UI.

// Implement scoring for the selected cards  
// Implement a table that holds the number of chips per ante and per round
// There is a chip multiplier for each special blind
// Grab the special blind for the round and apply the effect to the round as well as get the
// chip amount that the player needs to win the round

//using System.Diagnostics;
using UnityEngine;

public class Round
{
    private static Round instance;  //Player instance varaiable

    //Singleton for the player
    public static Round access()
    {
        if (instance == null)
        {
            instance = new Round();
        }

        return instance;
    }

    public Game game;
    public double targetScore;  //Target score for the current Ante and round
    public int[] baseAnteChips = { 0, 300, 800, 2000, 5000, 11000, 20000, 35000, 50000 };  //This contains the base chip amount per Ante in the game.
    public bool endGame;        //This bool indicates if the game has ended
    public string currentHand = ""; //  This is the current hand that gets updated by currentHandManager
    private Player player;

    
    //Implement logic here for the selected blind that the player selected.


    //Implement logic for calculating the score of the selected hand when the player clicks on the playHand button

    //This function takes in the currentAnte and current round the player is on
    //and returns the target number of chips for that round
    public double GetTargetScore(int currentAnte, int currentRound)
    {
        //This is used for debugging purposes to ensure the the values selected are within bounds
        if (currentRound > 3 || currentAnte > 8)
        {
            Debug.Log("Current round or current ante is not within bounds.");
            return 0;
        }

        //This switch statement determines the target score based on the blind value(Round value)
        //and current Ante that the player is currently playing.
        switch (currentRound)
        {
            case 1:
                targetScore = baseAnteChips[currentAnte] * 1;
                return targetScore;
            case 2:
                targetScore = baseAnteChips[currentAnte] * 1.5;
                return targetScore;
            case 3:
                targetScore = baseAnteChips[currentAnte] * Game.access().currentSpecialBlind.chipMultiplier;
                return targetScore;
            default:
                return 0;
        }

    }

    //This function is responsible for decreasing the amount of hands a player can play
    public int DecreaseHandCount()
    {
        player = Player.access();
        Debug.Log($"Hand count value: {player.handCount}");
        player.handCount = player.handCount - 1;
        return player.handCount;
    }

    //This function is responsible for decreasing the discards for the player
    public int DecreaseDiscardCount()
    {
        player = Player.access();
        player.discards -= 1;
        return player.discards;
    }


    //Implement a function that handles cleaning up all the variables when the user loses the game.
    public void RestartGame()
    {
        PlayerPrefsManager.ClearAll();
    }

    //The round should also have a way where players can sell their Mentors/Consumables
}