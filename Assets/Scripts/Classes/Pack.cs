// This Document contains the code for the Pack class
// This class contains information to form a Pack Screen 
// Current Devs:
// Robert (momomonkeyman): made class and variables

using System.Collections;
using UnityEngine;

// Packs will change the screen to a new screen where a player chooses cards
// from a select number and uses or adds them to the deck or mentorDeck
public class Pack
{
  public:
    PackType name;
    int packSize; // number of cards in the pack
    int PackDraw; // number of cards a player can choose
    int price;
}
