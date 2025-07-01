// This Document contains the code for the Game class.
// This class is used to hold information about the game's state
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Andy (flakkid): Added class and the editionRates dictionary
// Zacharia Alaoui: Added the functions and implemented the dictionaries

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardModifier
{
    private static System.Random rand = new System.Random();

    //  Rates of editions out of 100 
    public static Dictionary<int, CardEdition> editionRates = new()
    {
        { 60, CardEdition.Base },
        { 18, CardEdition.Foil },
        { 10, CardEdition.Holographic },
        { 7, CardEdition.Polychrome },
        { 5, CardEdition.Negative }
    };

    public static Dictionary<int, CardEnhancement> enhancementRates = new()
    {
        { 40, CardEnhancement.Base },
        { 18, CardEnhancement.BonusCard },
        { 15, CardEnhancement.MultCard },
        { 11, CardEnhancement.WildCard },
        { 8, CardEnhancement.GlassCard },
        { 5, CardEnhancement.SteelCard },
        { 3, CardEnhancement.GoldCard }
    };

    public static Dictionary<int, CardSeal> sealRates = new()
    {
        { 50, CardSeal.Base },
        { 20, CardSeal.Funding },
        { 18, CardSeal.Retake },
        { 12, CardSeal.Study }
    };

    //This function is used to retrieve a card modifier based on weighted values.
    //This function will return the card modifier based on the weighted values.
    public static T GetWeightedModifier<T>(Dictionary<int, T> weightedPool)
    {
        int totalWeight = weightedPool.Sum(kvp => kvp.Key);
        int randomValue = Random.Range(0, totalWeight);

        int currentWeight = 0;
        foreach (var kvp in weightedPool)
        {
            currentWeight += kvp.Key;
            if (randomValue <= currentWeight)
            {
                return kvp.Value;
            }
        }

        return weightedPool.First().Value; // Fallback if something goes wrong
    }



    //  Return additional price based on edition, 0 if base
    public static int EditionPrice(CardEdition edition)
    {
        switch (edition)
        {
            case CardEdition.Foil:
                return 2;
            case CardEdition.Holographic:
                return 3;
            case CardEdition.Polychrome:
                return 5;
            case CardEdition.Negative:
                return 5;
            default:
                return 0;
        }
    }

    // Return description for given card enhancement

    // Return description for given card edition

    // Return description for given card seal

    // Use enhancement effect with given use location

    // Use edition effect with given use location

    // Use seal effect with given use location
}
