// This Document contains the code for the Card class.
// This class is used to hold information about the cards that
// the player has inthe their deck or are going to choose from a pack
// Current Devs:
// Robert (momomonkeyman): made base class with the variables

using System.Collections;
using UnityEngine;

// Card inherits from BaseCard
public class PCard : BaseCard
{
    public CardType kindOfCard;
    public char linguisticChar;
    public SuitName suit;
    public PlaceArticulation placeArt;
    public MannerArticulation mannerArt;
    public bool isDiphthong;
    public int chips;
    public int multiply;
    public CardEdition edition;
    public CardEnhancement enhancement;
    public CardSeal seal;
    public bool isDisabled;
}
