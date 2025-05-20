// This Document contains the code for the Mentor interface
// This interface is the base for all of the mentor cards which will be 
// contained in the Mentor directory which is located in this directory
// Current Devs:
// Robert (momomonkeyman): made class and variables

using System.Collections;
using UnityEngine;

// Mentors can fundamentally alter the way a player will be able to play BELLEtro.
// Their effects can range from giving money, to multuplying the mult in a round.
public class Mentor
{
  public:
    int sellValue;
    int price;
    MentorName name;
    CardEdition edition;
    UseLocation[] locations;

    public void useMentor (Game game)
    {

    }
}
