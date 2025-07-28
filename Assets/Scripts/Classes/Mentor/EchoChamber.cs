// This Document contains the code for the EchoChamber mentor
// Effect is X2 Mult if hand has been repeated consecutively this round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class EchoChamber : Mentor
{
    private string prevHand = "None";

    //  Mentor name and basePrice are preset
    public EchoChamber(CardEdition edition) : base(MentorName.EchoChamber, edition, 8)
    {
        locations = new UseLocation[] { UseLocation.Post };
        description = "<b><color=red>X2 Mult</color></b> if hand has been repeated consecutively this round";
    }

    // X2 Mult if hand has been repeated consecutively this round
    public override void UseMentor()
    {
        string playedHand = ScoringManager.access().GetCurrentHandType();

        //  Won't work on the first hand 
        if(playedHand.Equals(prevHand))
        {
            //  Apply X2 Mult if same as prevHand
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 2);
            ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, $"<color=red>X2 Mult</color>"));
        }

        //  Set prev hand to playedHand
        prevHand = playedHand;
    }

    public void ResetPrevHand()
    {
        prevHand = "None";
    }
}
