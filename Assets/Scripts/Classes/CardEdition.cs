// This Document contains the code for the CardEdition Enum.
// This Enum contains the names for the editions of the cards
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// The card Edition is the quality of the card
public enum CardEdition
{
    Base,         // No change
    Foil,         // +50 chips
    Holographic,  // +10 Mult
    Polychrome,   // *1.5 Mult
    Negative      // +1 Mentor or Consumable slot (cannot be on Card)
}
