// This Document contains the code for the Blackboard mentor
// Effect is X3 Mult if all cards held in hand are Voiced or Tense
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class Blackboard : Mentor
{
    Deck deck = Deck.access();

    //  Mentor name and basePrice are preset
    public Blackboard(CardEdition edition) : base(MentorName.Blackboard, edition, 6)
    {
        locations = new UseLocation[] {UseLocation.Post };
        description = "<b><color=red>X3 Mult</color></b> if all cards held in hand are <b>Voiced</b> or <b>Tense</b>";
    }

    // X3 Mult if all cards held in hand are Voiced or Tense
    public override void UseMentor()
    {
        bool allVoicedTense = true;

        //  Check through held hand, mark condition false if Voiceless or Lax, and not enabled Wild card
        foreach(PCard heldCard in deck.heldHand)
        {
            if((heldCard.suit == SuitName.Voiceless || heldCard.suit == SuitName.Lax) && !(!heldCard.isDisabled && heldCard.enhancement == CardEnhancement.WildCard))
            {
                allVoicedTense = false;
                break;
            }
        }

        if (allVoicedTense)
        {
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 3);
        }
    }
}
