// This Document contains the code for the HistoryClass mentor
// Effect is if the first hand of the round is 1 card, then add another copy of that card to the deck
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class HistoryClass : Mentor
{
    private bool isFirstHand = true;
    Deck deck = Deck.access();
    DeleteCard deleteScript;

    //  Mentor name and basePrice are preset
    public HistoryClass(CardEdition edition) : base(MentorName.HistoryClass, edition, 8)
    {
        locations = new UseLocation[] { UseLocation.AllCards, UseLocation.Post };
        description = "If first hand of round is 1 card, then add a copy of that card to the deck";
    }

    //  To indicate whether effect will work with selected hand
    public override string GetDescription()
    {
        if(deleteScript == null)
        {
            deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        }

        int count = deleteScript.GetSelectedCards().Count;

        if(count == 1 && isFirstHand)
        {
            description = "If first hand of round is 1 card, then add a copy of that card to the deck (active)";
        }
        else
        {
            description = "If first hand of round is 1 card, then add a copy of that card to the deck (inactive)";
        }
        return description;
    }

    // If first hand of the round is 1 card, then add another copy of that card to the deck
    public override void UseMentor()
    {
        if (ScoringManager.access().GetScoringStatus())
        {
            //  Mark as not first hand
            isFirstHand = false;
        }
        else
        {
            //  Clone first card
            List<PCard> scoredPCards = ScoringManager.access().GetScoredPCards();
            if (isFirstHand && scoredPCards.Count == 1)
            {
                Debug.Log("Card cloned!");
                deck.AddCard(scoredPCards[0]);
            }
        }
    }

    //  At end of round, reset status for isFirstHand bool
    public void ResetStatus()
    {
        isFirstHand = true;
    }
}
