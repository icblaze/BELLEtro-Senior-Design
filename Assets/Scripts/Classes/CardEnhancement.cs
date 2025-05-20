// This Document contains the code for the CardEnhancement Enum.
// This Enum contains the names for the CardEnhancements
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// Enhancemetns are a type of effect that a card can have
public enum CardEnhancement
{
    Base,        // No Change
    BonusCard,   // +30 Chips
    MultCard,    // +4 Mult
    WildCard,    // Card is considered to be in all suits
    GlassCard,   // *2 Mult and 25% chance of breaking
    SteelCard,   // *1.5 Mult when hold in hand
    GoldCard     // Given $3 if held in hand at end of round
}
