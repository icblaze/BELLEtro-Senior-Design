﻿// This Document contains the code for the Curve Mentor.
// Prevents loss if chips scored at least required 25%. Disappears after use.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Numerics;
using System;


public class MadHatter : Mentor
{
    private Player player = Player.access();
    private JokerCardHolder mentorCardHolder;
    private float xmult = 1;

    //  Mentor name and basePrice are preset
    public MadHatter(CardEdition edition) : base(MentorName.MadHatter, edition, 7)
    {
        //  Don't have to include UseLocation.Blind since will check in SmallBlind and BigBlind
        locations = new UseLocation[] { UseLocation.Post };
        description = $"When Small or Big Blind is selected, gain X0.5 Mult and another random Mentor is destroyed (X1 Mult)";
    }

    //  Change the xmult
    public override string GetDescription()
    {
        description = $"When Small or Big Blind is selected, gain <b><color=red>X0.5 Mult</color></b> and another random Mentor is destroyed (<b><color=red>X{xmult} Mult</color></b>)";
        return description;
    }

    //  Apply the XMult
    public override void UseMentor()
    {
        int newMult = (int)Math.Floor(ScoringManager.access().GetCurrentMult() * xmult);
        ScoringManager.access().SetCurrentMult(newMult);
        ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>X{xmult} Mult</color></b>"));
    }

    //  Call when Small or Big Blind selected
    public void IncreaseMult()
    {
        //  Means that other mentors besides this MadHatter
        if(player.mentorDeck.Count > 1)
        {
            int madHatterIndex = player.mentorDeck.IndexOf(this);
            int destroyIndex = madHatterIndex;

            //  Randomize until not MadHatter
            while(madHatterIndex == destroyIndex)
            {
                destroyIndex = UnityEngine.Random.Range(0, player.mentorDeck.Count);
            }

            // Get Mentor to be destroyed
            Mentor destroyMentor = player.mentorDeck[destroyIndex];

            //  Remove from player's mentorDeck
            mentorCardHolder = GameObject.FindFirstObjectByType<JokerCardHolder>();
            mentorCardHolder.RemoveMentor(destroyMentor);

        }

        //  Gain X0.5 Mult
        xmult += 0.5f;
        ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<b><color=red>Increase XMult</color></b>"));
    }
}
