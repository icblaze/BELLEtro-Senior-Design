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
        switch (name)
        {
            case MentorName.CheatSheet:
                return new CheatSheet(cardEdition);
            case MentorName.ExtraCredit:
                return new ExtraCredit(cardEdition);
            case MentorName.BonusPoints:
                return new BonusPoints(cardEdition);
            case MentorName.Curve:
                return new Curve(cardEdition); 
            case MentorName.HelpingHand:
                return new HelpingHand(cardEdition);
            case MentorName.Brainstorm:
                return new Brainstorm(cardEdition);
            case MentorName.LabGlasses:
                return new LabGlasses(cardEdition); 
            case MentorName.Extension:
                return new Extension(cardEdition);
            case MentorName.LibraryCard:
                return new LibraryCard(cardEdition);
            case MentorName.TwelveCredits:
                return new TwelveCredits(cardEdition); 
            case MentorName.Valentine:
                return new Valentine(cardEdition); 
            case MentorName.NoVoice:
                return new NoVoice(cardEdition); 
            case MentorName.Eyes:
                return new Eyes(cardEdition);   
            case MentorName.Astronaut:
                return new Astronaut(cardEdition);
            case MentorName.FishBait:
                return new FishBait(cardEdition);   
            case MentorName.Triplets:
                return new Triplets(cardEdition);  
            case MentorName.Graduate:
                return new Graduate(cardEdition);   
            case MentorName.Vagabond:
                return new Vagabond(cardEdition);
            case MentorName.Turtle:
                return new Turtle(cardEdition);    
            case MentorName.Consonant:
                return new Consonant(cardEdition);  
            case MentorName.Diphcotomy:
                return new Diphcotomy(cardEdition); 
            case MentorName.ELLEvation:
                return new Ellevation(cardEdition); 
            case MentorName.MakeupExam:
                return new MakeupExam(cardEdition);
            case MentorName.EvenAirflow:
                return new EvenAirflow(cardEdition); 
            case MentorName.TheUsualSpot:
                return new TheUsualSpot(cardEdition);
            case MentorName.CrossItOut:
                return new CrossItOut(cardEdition);
            case MentorName.PageFlip:
                return new PageFlip(cardEdition);
            case MentorName.GradingWeights:
                return new GradingWeights(cardEdition);
            case MentorName.AreaOfExpertise:
                return new AreaOfExpertise(cardEdition);
            case MentorName.Glider:
                return new Glider(cardEdition);
            case MentorName.MindMeld:
                return new MindMeld(cardEdition);
            case MentorName.EchoChamber:
                return new EchoChamber(cardEdition);
            case MentorName.FastLearner:
                return new FastLearner(cardEdition);
            case MentorName.Overachiever:
                return new Overachiever(cardEdition);
            case MentorName.Daydreamer:
                return new Daydreamer(cardEdition);
            case MentorName.Revisionist:
                return new Revisionist(cardEdition);
            case MentorName.WildCard:
                return new WildCard(cardEdition);
            case MentorName.LinguistsEdge:
                return new LinguistsEdge(cardEdition);
            case MentorName.FrequencyHopper:
                return new FrequencyHopper(cardEdition);
            case MentorName.LateBloomer:
                return new LateBloomer(cardEdition);
            case MentorName.DiphthongDelight:
                return new DiphthongDelight(cardEdition);
            case MentorName.FirstInLine:
                return new FirstInLine(cardEdition);
            case MentorName.Blackboard:
                return new Blackboard(cardEdition);
            case MentorName.HistoryClass:
                return new HistoryClass(cardEdition);
            case MentorName.Recess:
                return new Recess(cardEdition);

            default:
                return new Mentor();
        }
    }

    //  placeholder default constructor (debug "None" mentor, base edition)
    public Mentor()
    {
        name = MentorName.None;
        edition = CardEdition.Base;
        locations = new UseLocation[] { UseLocation.Post };

        price = 2;
        sellValue = 1;
        description = "+4 Mult";
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
