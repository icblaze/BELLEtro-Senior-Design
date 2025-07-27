// This Document contains the code for the LateBloomer mentor
// This Mentor gives X3 Mult after Ante 4
// Current Devs:
// Andy (flakkid): Created mentor

using UnityEngine;
using System.Collections;


public class LateBloomer : Mentor
{
    Game gameInst = Game.access();

    //  Mentor name and basePrice are preset
    public LateBloomer(CardEdition edition) : base(MentorName.LateBloomer, edition, 8)
    {
        locations = new UseLocation[] {UseLocation.Post };
        description = "<b><color=red>X3 Mult</color></b> after Ante 4 (<i>inactive</i>)";
    }

    //  Update description with active status
    public override string GetDescription()
    {
        if(gameInst.GetAnte() > 4)
        {
            description = "<b><color=red>X3 Mult</color></b> after Ante 4 (<i>active</i>)";
        }
        else
        {
            description = "<b><color=red>X3 Mult</color></b> after Ante 4 (<i>inactive</i>)";
        }
        return description;
    }

    //  X3 Mult after Ante 4
    public override void UseMentor()
    {
        if(gameInst.GetAnte() > 4)
        {
            ScoringManager.access().SetCurrentMult(ScoringManager.access().GetCurrentMult() * 3);
        }
    }
}
