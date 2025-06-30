// This Document contains the code for the MakeupExam mentor
// This Mentor effect is to Retrigger all cards for the next 10 hands.
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class MakeupExam : Mentor
{
    Player player = Player.access();
    int retriggerUses = 10;

    //  Mentor name and basePrice are preset
    public MakeupExam(CardEdition edition) : base(MentorName.MakeupExam, edition, 6)
    {
        locations = new UseLocation[] { UseLocation.PreCard, UseLocation.PostHand};
        description = "Retrigger all cards for the next 10 hands";
    }

    //  Update description with uses left
    public override string GetDescription()
    {
        description = "Retrigger all cards for the next " + retriggerUses + " hands";
        return description;
    }

    //  Add retrigger count to each scored pcard (PreCard)
    public override void UseMentor(PCard pcard)
    {
        pcard.replayCounter++;
    }

    //  Decrement uses left after each hand played (PostHand)
    public override void UseMentor()
    {
        retriggerUses--;

        //  Disappears after all uses 
        if(retriggerUses == 0)
        {
            player.mentorDeck.Remove(this);

            //TODO Remove visually
        }
    }

}
