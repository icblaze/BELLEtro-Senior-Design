// This Document contains the code for the MannerArticulation Enum.
// This Enum contains the names for the Manners of Articulation needed
// to identify the linguistic characters
// Current Devs:
// Robert (momomonkeyman): made enum

using System.Collections;
using UnityEngine;

// Manner of Articulation refers to the way of how the airflow flows through
// the vocal tract to produce a sound.
public enum MannerArticulation
{
    Stop,
    Fricative,
    Affricate,
    Nasal,
    Liquid,
    Glide,

//The following terms describe the tongue height when pronouncing a vowel
    High,                           
    Mid,
    Low
}
