// This Document contains the code for the Card class.
// This class is used to hold information about the cards that
// the player has in their deck or are going to choose from a pack.
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Zacharia Alaoui (ZachariaAlaoui): Made the functions and Copy constructor

using System.Collections;
using UnityEngine;

// Card inherits from BaseCard
public class PCard : BaseCard
{
    private CardType _kindOfCard;                   // This variable will be used to set the kindOfCard property
    public LinguisticTerms term;                    // Enum that contains all the Linguistic Terms in the deck
    public SuitName suit;                           // This enum contains the suits in the deck
    public PlaceArticulation placeArt;              // Place of articulation 
    public MannerArticulation mannerArt;            // Manner of articulation
    public bool isDiphthong;                        // Bool to see if the vowel is a diphthong
    public int chips;                               // Number of chips based on the card
    public int multiplier;                          // Multiplier of the card
    public CardEdition edition;                     // Edition of the card
    public CardEnhancement enhancement;             // Card enhancement of the card  
    public CardSeal seal;                           // Card seal on the card
    public bool isDisabled;                         // isDisabled is used to indicate if a card is able to be used
    public int replayCounter;                       // Number of times card is replayed, modified by seal or Mentors
    public int cardID;                                  // Unique identification for each PCard

    public Mentor mentor;                       // Mentor card
    public Textbook textbook;                   // Textbook card
    public CardBuff cardBuff;                   // Card buff card


    //Default constructor for PCard
    public PCard()
    {}

    //Copy constructor for PCard
    public PCard(PCard original)
    {
        this.kindOfCard = original.kindOfCard;
        this.term = original.term;
        this.suit = original.suit;
        this.placeArt = original.placeArt;
        this.mannerArt = original.mannerArt;
        this.isDiphthong = original.isDiphthong;
        this.chips = original.chips;
        this.multiplier = original.multiplier;
        this.edition = original.edition;
        this.enhancement = original.enhancement;
        this.seal = original.seal;
        this.isDisabled = original.isDisabled;
    }

    public static PCard CloneCard(PCard card)
    {
        PCard clonedCard = new PCard(card);
        return clonedCard;
    }

    public bool Equals(PCard other)
    {
        return suit == other.suit &&
               term == other.term &&
               placeArt == other.placeArt &&
               mannerArt == other.mannerArt &&
               edition == other.edition &&
               enhancement == other.enhancement &&
               seal == other.seal;
    }

    public override CardType kindOfCard
    {
        get { return _kindOfCard; }
        set { _kindOfCard = value; }
    } //Getter and setter for typeOfCard

    //  Return attributes of the PCard (chips, mult, and card modifiers if any)
    public override string ToString()
    {
        //  For testing/debugging
        string description = "";

        description += $"<color=blue>+{chips} Chips</color> <color=red>+{multiplier} Mult</color>";
        description += CardModifier.access().EnhancementDesc(enhancement);
        description += CardModifier.access().SealDesc(seal);
        description += CardModifier.access().EditionDesc(edition);

        return description;
    }
}

