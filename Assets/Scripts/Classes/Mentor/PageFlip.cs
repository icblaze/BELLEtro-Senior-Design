// This Document contains the code for the PageFlip mentor
// Effect is to have +1 Discard each round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class PageFlip : Mentor
{
    DeleteCard deleteScript;

    //  Mentor name and basePrice are preset
    public PageFlip(CardEdition edition) : base(MentorName.PageFlip, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.Blind };
        description = "<color=orange>+1</color> Discard each round";
    }

    //  +1 Discard each round
    public override void UseMentor()
    {
        if (deleteScript == null)
        {
            deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        }

        deleteScript.SetDiscards(Player.access().discards + 1);
        ScoreCoroutine(ScoringManager.access().ScorePopupMentor(this, "<color=orange>+1 Discard</color>"));
    }
}
