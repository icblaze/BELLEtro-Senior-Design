//  This Document contains the code for managing the
//  Mentor buffers and allowing for interaction with other
//  objects for the gameflow

// Current Devs:
// Andy (flakkid): Made intial buffers and assignment

using System.Collections.Generic;
using UnityEngine;

public class MentorBufferManager
{
    public static Dictionary<UseLocation, List<Mentor>> mentorBuffers = new();


    //  Assigns each Mentor of the Player's mentorDeck to their buffer
    public static void AssignToBuffer (Game game)
    {
        foreach (UseLocation location in System.Enum.GetValues(typeof(UseLocation)))
        {
            mentorBuffers[location] = new List<Mentor>();
        }

        foreach (Mentor mentor in game.ThePlayer.mentorDeck)
        {
            foreach (UseLocation location in mentor.locations)
            {
                mentorBuffers[location].Add(mentor);
            }
        }
    }

    //  Execute buffer of specified location
    public static void RunBuffer (Game game, UseLocation buffer)
    {
        foreach (Mentor mentor in mentorBuffers[buffer])
        {
            mentor.UseMentor(game);
        }
    }

    //  Playing hand buffer execution
    public static void PlayHand (Game game)
    {
        //  Putting effect cards in their assigned buffer
        AssignToBuffer(game);

        //  Using Initial Effect Cards
        RunBuffer(game, UseLocation.Initial);

        //  Playing Each Card in Hand
        //  TODO Differentiate between playHand and drawHand (change the outer foreach loop)
        foreach (PCard card in game.ThePlayer.hand.playerHand)
        {
            if(card.isDisabled)
            {
                continue;
            }

            RunBuffer(game, UseLocation.PreCard);
            //  TODO Play Card call here
            RunBuffer(game, UseLocation.PostCard);
        }

        //  Playing From-Draw Cards
        foreach (PCard card in game.ThePlayer.hand.playerHand)
        {
            if (card.isDisabled)
            {
                continue;
            }

            RunBuffer(game, UseLocation.PreFromDraw);
            //  TODO Play From-Draw call here
            RunBuffer(game, UseLocation.PostFromDraw);
        }

        //  Using Post Effect Cards
        RunBuffer(game, UseLocation.Post);

        //  TODO Scoring phase here
    }

    
}
