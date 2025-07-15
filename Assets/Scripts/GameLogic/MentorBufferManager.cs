//  This Document contains the code for managing the
//  Mentor buffers and allowing for interaction with other
//  objects for the gameflow

// Current Devs:
// Andy (flakkid): Made intial buffers and assignment

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorBufferManager
{
    public static Dictionary<UseLocation, List<Mentor>> mentorBuffers = new();
    private Player player = Player.access();
    private Game game = Game.access();

    //  Adjust time of scoring manager between each score increment
    private readonly float waitIncrement = 0.5f;

    //  Make this a singleton
    private static MentorBufferManager instance;
    public static MentorBufferManager access()
    {
        if (instance == null)
        {
            instance = new MentorBufferManager();
        }
        return instance;
    }

    //  Assigns each Mentor of the Player's mentorDeck to their buffer
    public void AssignToBuffer ()
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

        //  Debug key-value pairs of mentorBuffers
        foreach (var kvp in mentorBuffers)
        {
            string debugList = $"Mentors for {kvp.Key}: ";
            foreach (var mentor in kvp.Value)
            {
                debugList += mentor.name + ", ";
            }
            Debug.Log(debugList);
        }
    }

    //  Execute buffer of specified location
    public IEnumerator RunBuffer (UseLocation buffer)
    {
        foreach (Mentor mentor in mentorBuffers[buffer])
        {
            mentor.UseMentor();
            yield return new WaitForSecondsRealtime(waitIncrement);
        }
    }

    //  Execute buffer of specified location with no delay
    public void RunBufferImmediate(UseLocation buffer)
    {
        foreach (Mentor mentor in mentorBuffers[buffer])
        {
            mentor.UseMentor();
        }
    }

    //  Execute buffer of specified location that require a specific card
    public IEnumerator RunBuffer(UseLocation buffer, PCard card)
    {
        foreach (Mentor mentor in mentorBuffers[buffer])
        {
            mentor.UseMentor(card);
            yield return new WaitForSecondsRealtime(waitIncrement);
        }
    }

    //  Execute buffer for setting retriggers (don't wait here)
    public IEnumerator RunRetriggerBuffer(List<PCard> selectedPCards)
    {
        foreach (Mentor mentor in mentorBuffers[UseLocation.Retrigger])
        {
            mentor.UseRetriggerMentor(selectedPCards);
        }
        yield return null;
    }

    //  Go through Post Buffer in left to right order, considering the edition of the mentors
    public IEnumerator RunPostBuffer()
    {
        //  Go through player's mentor deck from left to right
        foreach (Mentor mentor in player.mentorDeck)
        {
            yield return CardModifier.access().UseMentorEdition(mentor, false);

            //  Use mentor's effect if in Post UseLocation
            if(mentorBuffers[UseLocation.Post].Contains(mentor))
            {
                mentor.UseMentor();
                yield return new WaitForSecondsRealtime(waitIncrement);
            }

            yield return CardModifier.access().UseMentorEdition(mentor, true);
        }
    }

    //  Reset certain mentors whose effect only work once per blind or reset at end
    public void ResetMentorStatus()
    {
        foreach (Mentor mentor in player.mentorDeck)
        {
            switch (mentor.name)
            {
                //  Change suit after each round
                case MentorName.GradingWeights:
                    GradingWeights gradingWeights = (GradingWeights)mentor;
                    gradingWeights.RandomizeSuit();
                    break;

                //  Reset hash set each round
                case MentorName.MindMeld:
                    MindMeld mindMeld = (MindMeld)mentor;
                    mindMeld.EmptyHandSet();
                    break;

                //  Set prevHand to "None"
                case MentorName.EchoChamber:
                    EchoChamber echoChamber = (EchoChamber)mentor;
                    echoChamber.ResetPrevHand();
                    break;

                //  Reset first hand status
                case MentorName.Overachiever:
                    Overachiever overachiever = (Overachiever)mentor;
                    overachiever.ResetStatus();
                    break;
            }
        }
    }
}
