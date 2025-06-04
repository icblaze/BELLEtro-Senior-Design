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
    public CardType kindOfCard;
    public LinguisticTerms term;                    //Enum that contains all the Linguistic Terms in the deck
    public SuitName suit;                           //This enum contains the suits in the deck
    public PlaceArticulation placeArt;              //Place of articulation 
    public MannerArticulation mannerArt;            //Manner of articulation
    public bool isDiphthong;
    public int chips;
    public int multiplier;
    public CardEdition edition;                     //Edition of the card
    public CardEnhancement enhancement;             //Card enhancement of the card  
    public CardSeal seal;                           //Card seal on the card
    public bool isDisabled;
}
