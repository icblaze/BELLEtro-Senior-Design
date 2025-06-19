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
        //This creates a list of mentors for each location that is in the UseLocation enum
        foreach (UseLocation location in System.Enum.GetValues(typeof(UseLocation)))
        {
            mentorBuffers[location] = new List<Mentor>();
        }

        //Go through all the mentors in the players deck
        foreach (Mentor mentor in player.mentorDeck)
        {
            //Add mentor to the mentorBuffer dictionary based on location
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

    //  Execute buffer of specified location that require a specific card
    public static void RunBuffer(UseLocation buffer, PCard card)
    {
        foreach (Mentor mentor in mentorBuffers[buffer])
        {
            mentor.UseMentor(card);
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
        Deck deck = Deck.access();
        foreach (PCard card in deck.playerHand)
        {
            if (card.isDisabled)
            {
                continue;
            }

            int replayCounter = 0;

            do
            {
                RunBuffer(UseLocation.PreCard, card);
                //  TODO Play Card call here
                RunBuffer(UseLocation.PostCard, card);
                replayCounter--;
            }while(replayCounter >= 0);
        }

        //  Playing From-Draw Cards (could track "not selected"?)
        foreach (PCard card in deck.playerHand)
        {
            if (card.isDisabled)
            {
                continue;
            }

            RunBuffer(UseLocation.PreFromDraw, card);
            //  TODO Play From-Draw call here
            // RunBuffer(UseLocation.PostFromDraw);
        }

        //  Using Post Effect Cards
        RunBuffer(UseLocation.Post);

        //  TODO Scoring phase here
    }

    
}
