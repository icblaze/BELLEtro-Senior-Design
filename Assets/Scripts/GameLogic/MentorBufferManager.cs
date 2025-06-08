//  This Document contains the code for managing the
//  Mentor buffers and allowing for interaction with other
//  objects for the gameflow

// Current Devs:
// Andy (flakkid): Made intial buffers and assignment

using System.Collections.Generic;
using UnityEngine;

public class MentorBufferManager : MonoBehaviour
{
    public static Dictionary<UseLocation, List<Mentor>> mentorBuffers = new();
    private static Player player = Player.access();
    private static Game game = Game.access();


    //  Assigns each Mentor of the Player's mentorDeck to their buffer
    public static void AssignToBuffer ()
    {
        foreach (UseLocation location in System.Enum.GetValues(typeof(UseLocation)))
        {
            mentorBuffers[location] = new List<Mentor>();
        }

        foreach (Mentor mentor in player.mentorDeck)
        {
            foreach (UseLocation location in mentor.locations)
            {
                mentorBuffers[location].Add(mentor);
            }
        }
    }

    //  Execute buffer of specified location
    public static void RunBuffer (UseLocation buffer)
    {
        foreach (Mentor mentor in mentorBuffers[buffer])
        {
            mentor.UseMentor();
        }
    }

    //  Playing hand buffer execution
    public static void PlayHand ()
    {
        //  Putting effect cards in their assigned buffer
        AssignToBuffer();

        //  Using Initial Effect Cards
        RunBuffer(UseLocation.Initial);

        //  Playing Each Card in Hand
        //  TODO Differentiate between playHand and drawHand (change the outer foreach loop)
        foreach (PCard card in game.ThePlayer.deck.playerHand)
        {
            if(card.isDisabled)
            {
                continue;
            }

            RunBuffer(UseLocation.PreCard);
            //  TODO Play Card call here
            RunBuffer(UseLocation.PostCard);
        }

        //  Playing From-Draw Cards (could track "not selected"?)
        foreach (PCard card in game.ThePlayer.deck.playerHand)
        {
            if (card.isDisabled)
            {
                continue;
            }

            RunBuffer(UseLocation.PreFromDraw);
            //  TODO Play From-Draw call here
            // RunBuffer(UseLocation.PostFromDraw);
        }

        //  Using Post Effect Cards
        RunBuffer(UseLocation.Post);

        //  TODO Scoring phase here
    }

    
}
