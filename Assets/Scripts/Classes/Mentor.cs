// This Document contains the code for the Mentor interface
// This interface is the base for all of the mentor cards which will be 
// contained in the Mentor directory which is located in this directory
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Andy (flakkid): constructor and method to be overriden

using System.Collections;
using System.Collections.Generic;
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
    public bool isDisabled;

    //  This will construct the appropiate Mentor subclass with passed card edition
    public static Mentor MentorFactory(MentorName name, CardEdition cardEdition)
    {
        switch(name)
        {
            case MentorName.CheatSheet:
                return new CheatSheet(cardEdition);
            case MentorName.ExtraCredit:
                return new ExtraCredit(cardEdition);
            case MentorName.BonusPoints:
                return new BonusPoints(cardEdition);
            case MentorName.Curve:
                return new Curve(cardEdition); //  TODO Delete Mentor in UI
            case MentorName.HelpingHand:
                return new HelpingHand(cardEdition);
            case MentorName.Brainstorm:
                return new Brainstorm(cardEdition);
            case MentorName.LabGlasses:
                return new LabGlasses(cardEdition); //  TODO Assign the mult to the variable in round
            case MentorName.Extension:
                return new Extension(cardEdition); 
            case MentorName.LibraryCard:
                return new LibraryCard(cardEdition);
            case MentorName.TwelveCredits:
                return new TwelveCredits(cardEdition); // TODO Be able to track selected hand
            case MentorName.Valentine:
                return new Valentine(cardEdition); //  TODO add +3 to Mult variable of round
            case MentorName.NoVoice:
                return new NoVoice(cardEdition); //  TODO add +3 to Mult variable of round
            case MentorName.Eyes:
                return new Eyes(cardEdition);   //  TODO add +8 to Mult variable of round
            case MentorName.Astronaut:
                return new Astronaut(cardEdition);
            case MentorName.FishBait:
                return new FishBait(cardEdition);   //  TODO add +100 to Chip variable of round
            case MentorName.Triplets:
                return new Triplets(cardEdition);   //  TODO add +12 Mult variable of round
            case MentorName.Graduate:
                return new Graduate(cardEdition);   //  TODO +20 Chips and +4 Mult when Dipthongs are scored
            case MentorName.Vagabond:
                return new Vagabond(cardEdition);
            case MentorName.Turtle:
                return new Turtle(cardEdition);
            case MentorName.Consonant:
                return new Consonant(cardEdition);  //  TODO +30 Chips to round variable
            case MentorName.Diphcotomy:
                return new Diphcotomy(cardEdition); //  TODO Add +mult to the round variable
            case MentorName.ELLEvation:
                return new Ellevation(cardEdition); //  TODO Add +chips to round variable
            case MentorName.MakeupExam:
                return new MakeupExam(cardEdition);
            case MentorName.EvenAirflow:
                return new EvenAirflow(cardEdition); //  TODO Multiply the Round mult variable by 2
            case MentorName.TheUsualSpot:
                return new TheUsualSpot(cardEdition); //  TODO Gives +80 Chips if played hand matches by place of articulation

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
        description = "This is a test mentor with a really long description to test the bounds of this description box. This is a test mentor with a long description! long description! long description!";
    }

    //  Constructor that each Mentor will use as the base, locations filled out in specific mentor
    public Mentor (MentorName name, CardEdition edition, int basePrice)
    {
        this.name = name;
        this.edition = edition;
        locations = new UseLocation[]{};    // Create empty locations array
        isDisabled = false;

        //  Calculate price based on edition, and have sell value be half that amount
        price = basePrice + CardModifier.access().EditionPrice(edition);
        sellValue = (int)Mathf.Floor(price / 2.0f);
    }

    //  Method to override for each unique Mentor, activates effect
    public virtual void UseMentor ()
    {
        
    }

    //  Method to override for Mentors with card specific criteria
    public virtual void UseMentor (PCard card)
    {

    }

    //  Method to override for Mentors that deal with retriggers
    public virtual void UseRetriggerMentor(List<PCard> scoredPCards)
    {

    }

    //  Returns the description of mentor 
    public virtual string GetDescription()
    {
        return description;
    }
}
