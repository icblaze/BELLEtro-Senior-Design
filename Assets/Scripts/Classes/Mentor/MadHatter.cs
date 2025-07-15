// This Document contains the code for the Curve Mentor.
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
        description = "When a Small or Big Blind is selected, this Mentor gains X0.5 Mult and another Mentor is randomly destroyed (X1 Mult)";
    }

    //  Change the xmult
    public override string GetDescription()
    {
        description = $"When a Small or Big Blind is selected, this Mentor gains X0.5 Mult and another Mentor is randomly destroyed (X{xmult} Mult)";
        return description;
    }

    //  Apply the XMult
    public override void UseMentor()
    {
        int newMult = (int)Math.Floor(ScoringManager.access().GetCurrentMult() * xmult);
        ScoringManager.access().SetCurrentMult(newMult);
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
    }
}
