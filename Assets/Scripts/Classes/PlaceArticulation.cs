// This Document contains the code for the PlaceArticulation Enum.
// This Enum contains the names for the Places of Articulation needed
// to identify the linguistic characters
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// The place of articulation refers to the place in the vocal tract that produces the sound
public enum PlaceArticulation
{
    Labial,
    Labiodental,
    Interdental,
    Alveolar,
    AlveoPalatal,
    Velar,
    Glottal,

//The following terms refer to the place of the tongue when producing a vowel sound
    Front,
    Central,
    Back
}
