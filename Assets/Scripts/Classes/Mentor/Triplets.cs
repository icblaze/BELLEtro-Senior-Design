// This Document contains the code for the Triplets mentor
// +12 Mult if played hand contains three of a kind
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Triplets : Mentor
{

    //  Mentor name and basePrice are preset
    public Triplets(CardEdition edition) : base(MentorName.Triplets, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "<color=red>+12 Mult</color> if played hand contains three of a kind";
    }

    // +12 Mult if played hand contains three of a kind
    public override void UseMentor()
    {
        string playedHand = ScoringManager.access().GetCurrentHandType();

        //  Add +12 Mult to multiplier
        if (playedHand.Equals("ThreeKind") || playedHand.Equals("FullHouse") || playedHand.Equals("FourKind") || playedHand.Equals("FiveKind") || playedHand.Equals("FlushFive") | playedHand.Equals("FlushHouse"))
        {
            ScoringManager.access().IncrementCurrentMult(12);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=red>+12 Mult</color>"));
        }
    }
}
