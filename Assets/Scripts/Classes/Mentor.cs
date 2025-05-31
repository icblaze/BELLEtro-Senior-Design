// This Document contains the code for the Mentor interface
// This interface is the base for all of the mentor cards which will be 
// contained in the Mentor directory which is located in this directory
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakkid): constructor and method to be overriden

using System.Collections;
using UnityEngine;

// Mentors can fundamentally alter the way a player will be able to play BELLEtro.
// Their effects can range from giving money, to multiplying the mult in a round.
public class Mentor
{
    public int sellValue;
    public int price;
    public MentorName name;
    public CardEdition edition;
    public UseLocation[] locations;


    //  Default constructor
    public Mentor()
    {

    }

    //  Constructor that each Mentor will use as the base
    public Mentor (MentorName name, int price, int sellValue, CardEdition edition, UseLocation[] locations)
    {
        this.name = name;
        this.price = price;
        this.sellValue = sellValue;
        this.edition = edition;
        this.locations = locations;
    }

    //  Method to override for each unique Mentor, activates effect
    public virtual void UseMentor (Game game)
    {
        
    }
}
