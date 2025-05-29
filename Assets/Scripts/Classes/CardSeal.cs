// This Document contains the code for the CardSeal Enum.
// This Enum contains the names for types of seals that a card can have
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// The seal is an additional effect that can be on a base card only
public enum CardSeal
{
    Base,    // No seal
    Funding, // Grants $3 when card is scored
    Retake,  // Retriggers the card once
    Study    // Creates a Textbook which enhances the final played hand of round when 
}            // this card is held in hand at the end of the round
