// This Document contains the code for the Mentor interface
// This interface is the base for all of the mentor cards which will be 
// contained in the Mentor directory which is located in this directory
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakkid): constructor and method to be overriden

using System.Collections;
using UnityEngine;

// Mentors can fundamentally alter the way a player will be able to play BELLEtro.
// Their effects can range from giving money, to multuplying the mult in a round.
public class Mentor
{
    public int sellValue;
    public int price;
    public MentorName name;
    public CardEdition edition;
    public UseLocation[] locations;


    //  placeholder default constructor (will be "CheatSheet" mentor, base edition)
    public Mentor()
    {
        sellValue = 2;
        price = 4;

    }

    //  Constructor that each Mentor will use as the base, locations filled out in specific mentor
    public Mentor (MentorName name, CardEdition edition, int basePrice)
    {
        this.name = name;
        this.edition = edition;

        //  Calculate price based on edition, and have sell value be half that amount
        price = basePrice + PriceAssignment.EditionPrice(edition);
        sellValue = (int)Mathf.Floor(price / 2.0f);
    }

    //  Method to override for each unique Mentor, activates effect
    public virtual void UseMentor (Game game)
    {
        
    }
}
