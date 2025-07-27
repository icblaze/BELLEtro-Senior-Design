// This Document contains the code for the DiphthongDelight mentor
// This Mentor gives X2 Mult when Diphthong is scored
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class DiphthongDelight : Mentor
{
    private bool firstDipthong = true;

    //  Mentor name and basePrice are preset
    public DiphthongDelight(CardEdition edition) : base(MentorName.DiphthongDelight, edition, 5)
    {
        locations = new UseLocation[] {UseLocation.Initial, UseLocation.PostCard };
        description = "<b><color=red>X2 Mult</color></b> when first Diphthong is scored";
    }

    //  Reset firstDiphthong to true before scoring
    public override void UseMentor()
    {
        firstDipthong = true;
    }

    //  X2 Mult when first Diphthong is scored
    public override void UseMentor(PCard pcard)
    {
        if(firstDipthong && pcard.isDiphthong)
        {
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 2);
            firstDipthong = false;
        }
    }
}
