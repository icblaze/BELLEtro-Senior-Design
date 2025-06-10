// This Document contains the code for the Pack class
// This class contains information to form a Pack  
// Current Devs:
// Robert (momomonkeyman): made class and variables
// Zacharia Alaoui: Made additional variables, and created the functions that contains the switch statements

using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

// Packs will change the screen to a new screen where a player chooses cards
// from a select number and uses or adds them to the deck or mentorDeck
public class Pack
{
    public List<CardObject> cardsInPack = new List<CardObject>(); // List of cards in the pack
    public PackType packType;                                     // This variable holds a enum that contains all the different packs in the game 
    public int packSize;                                          // This variable will hold how many cards are in the pack
    public PackEdition edition;                                   // This variable indicates the size edition of the pack
    public int price;                                             // Price of the pack
    public int selectableCards;                                   // This variable will hold how many cards a player can pick from a pack



    //This function is responsible for getting the size of the pack, based on the edition.
    public int getPackSize(PackEdition packEdition)
    {
        switch (packEdition)
        {
            case PackEdition.Normal_Pack:
                return 3; //Standard packs contain 3 cards, and you can only select 1 card
            case PackEdition.Jumbo_Pack:
                return 5; //Jumbo packs contain 5 cards, however you can only select 1 card   
            case PackEdition.Mega_Pack:
                return 5; //Jumbo packs contain 5 cards, however you can select 2 cards
            default:
                Debug.Log("Unknown Pack type, inside getPackSize!");
                return 0; //Fallback case 
        }
    }

    //This function returns the price of the pack based on the pack edition.
    public int getPackPrice(PackEdition packEdition)
    {
        switch (packEdition)
        {
            case PackEdition.Normal_Pack:
                return 4;
            case PackEdition.Jumbo_Pack:
                return 6;
            case PackEdition.Mega_Pack:
                return 8;
            default:
                Debug.Log("Unknown Pack type, inside getPackPrice!");
                return 0; //Fallback case 
        }
    }

    //This function returns how many cards can be selected in a pack based on the edition passed in.
    public int getSelectableCount(PackEdition packEdition)
    {
        switch (packEdition)
        {
            case PackEdition.Normal_Pack:
                return 1;
            case PackEdition.Jumbo_Pack:
                return 1;
            case PackEdition.Mega_Pack:
                return 2;
            default:
                Debug.Log("Unknown Pack type, inside getSelectableCount!");
                return 0; //Fallback case 
        }
    }
}
