// This Document contains the code for the Card class.
// This class is used to hold information about the cards that
// the player has in their deck or are going to choose from a pack
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

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

    public MentorName mentor;                       // Mentor card
    public TextbookName textbook;                   // Textbook card
    public CardBuffName cardBuffName;               // Card buff card

    public override CardType kindOfCard
    {
        get { return _kindOfCard; }
        set { _kindOfCard = value; }
    } //Getter and setter for typeOfCard
}

