// This Document contains the code for the CrossItOut mentor
// Effect is to Upgrade the level of first discarded hand each round
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class CrossItOut : Mentor
{
    DeleteCard deleteScript;

    //  Mentor name and basePrice are preset
    public CrossItOut(CardEdition edition) : base(MentorName.CrossItOut, edition, 8)
    {
        locations = new UseLocation[] { UseLocation.Discard };
        description = "Upgrade the level of first discarded hand each round";
    }

    //  Get hand before discarding
    public override void UseMentor()
    {
        if(deleteScript == null)
        {
            deleteScript = GameObject.FindFirstObjectByType<DeleteCard>();
        }

        //  Find hand that is to be discarded
        string hand = CurrentHandManager.Instance.findCurrentHand(deleteScript.GetSelectedPCards());
        TextbookName tbookName = CardModifier.access().GetTextbookFromString(hand);

        //  If first discard of round (before decrement), increase level of discarded hand
        if(Player.access().discards == Player.access().maxDiscards)
        {
            Player.access().handTable[tbookName].IncreaseLevel();
        }
    }
}
