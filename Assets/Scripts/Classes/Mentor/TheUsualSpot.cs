// This Document contains the code for the TheUsualSpot mentor
// This Mentor effect is Gives +80 Chips if played hand matches by place of articulation
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheUsualSpot : Mentor
{
    Player player = Player.access();

    //  Mentor name and basePrice are preset
    public TheUsualSpot(CardEdition edition) : base(MentorName.TheUsualSpot, edition, 4)
    {
        locations = new UseLocation[] { UseLocation.Post};
        description = "Gives <color=blue>+80 Chips</color> if played hand matches by place of articulation";
    }

    //  Gives +80 Chips if played hand matches by place of articulation
    public override void UseMentor()
    {
        //  May have to change way of accessing played hand
        List<PCard> pcardList = ScoringManager.access().GetScoredPCards();
        int cardCount = pcardList.Count;

        //  No match if only 1 card
        if(cardCount <= 1)
        {
            return;
        }

        PlaceArticulation place = pcardList[0].placeArt;    //  Get first card's placeArt

        //  don't activate effect
        for (int i = 1; i < cardCount; i++)
        {
            if(pcardList[i].placeArt != place)
            {
                return;
            }
        }

        //  Gives +80 Chips if played hand matches by place of articulation
        ScoringManager.access().IncrementCurrentChips(80);
    }

}
