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
    public string description;

    //  This will construct the appropiate Mentor subclass with passed card edition
    public static Mentor MentorFactory(MentorName name, CardEdition cardEdition)
    {
        switch(name)
        {
            case MentorName.CheatSheet:
                return new CheatSheet(cardEdition);
            case MentorName.ExtraCredit:
                return new ExtraCredit(cardEdition);
            default:
                return new Mentor();
        }
    }

    //  placeholder default constructor (debug "None" mentor, base edition)
    public Mentor()
    {
        name = MentorName.None;
        edition = CardEdition.Base;
        locations = new UseLocation[] { UseLocation.Blind };

        price = 2;
        sellValue = 1;
        description = "This is a test mentor";
    }

    //  Constructor that each Mentor will use as the base, locations filled out in specific mentor
    public Mentor (MentorName name, CardEdition edition, int basePrice)
    {
        this.name = name;
        this.edition = edition;

        //  Calculate price based on edition, and have sell value be half that amount
        price = basePrice + CardModifier.EditionPrice(edition);
        sellValue = (int)Mathf.Floor(price / 2.0f);
    }

    //  Method to override for each unique Mentor, activates effect
    public virtual void UseMentor ()
    {
        
    }

    //  Returns the description of mentor 
    public virtual string GetDescription()
    {
        return description;
    }
}
