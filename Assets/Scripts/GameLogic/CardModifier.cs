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

    //  Make this a singleton
    private static CardModifier instance;
    public static CardModifier access()
    {
        if (instance == null)
        {
            instance = new CardModifier();
        }
        return instance;
    }

    //  Rates of editions out of 100 
    public Dictionary<int, CardEdition> editionRates = new()
    {
        { 60, CardEdition.Base },
        { 18, CardEdition.Foil },
        { 10, CardEdition.Holographic },
        { 7, CardEdition.Polychrome },
        { 5, CardEdition.Negative }
    };

    public Dictionary<int, CardEnhancement> enhancementRates = new()
    {
        { 40, CardEnhancement.Base },
        { 18, CardEnhancement.BonusCard },
        { 15, CardEnhancement.MultCard },
        { 11, CardEnhancement.WildCard },
        { 8, CardEnhancement.GlassCard },
        { 5, CardEnhancement.SteelCard },
        { 3, CardEnhancement.GoldCard }
    };

    public Dictionary<int, CardSeal> sealRates = new()
    {
        { 50, CardSeal.Base },
        { 20, CardSeal.Funding },
        { 18, CardSeal.Retake },
        { 12, CardSeal.Study }
    };

    //This function is used to retrieve a card modifier based on weighted values.
    //This function will return the card modifier based on the weighted values.
    public T GetWeightedModifier<T>(Dictionary<int, T> weightedPool)
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
    public int EditionPrice(CardEdition edition)
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
    public string EnhancementDesc(CardEnhancement enhancement)
    {
        switch(enhancement)
        {
            case CardEnhancement.Base:
                return "";
            case CardEnhancement.BonusCard:
                return "Bonus Card: +30 extra Chips";
            case CardEnhancement.MultCard:
                return "Mult Card: +4 Mult";
            case CardEnhancement.WildCard:
                return "Wild Card: Can be used as any suit";
            case CardEnhancement.GoldCard:
                return "Gold Card: Gain $3 if held in hand at end of round";
            case CardEnhancement.SteelCard:
                return "Steel Card: X1.5 Mult when held in hand";
            case CardEnhancement.GlassCard:
                return "Glass Card: X2 Mult when scored. 25% chance of being destroyed after scoring";
        }

        return "";
    }

    // Return description for given card edition
    public string EditionDesc(CardEdition edition)
    {
        switch (edition)
        {
            case CardEdition.Base:
                return "";
            case CardEdition.Foil:
                return "Foil: +50 Chips";
            case CardEdition.Holographic:
                return "Holographic: +10 Mult";
            case CardEdition.Polychrome:
                return "Polychrome: X1.5 Mult";
            case CardEdition.Negative:
                return "Negative: -1 Mentor Slot";
        }

        return "";
    }

    // Return description for given card seal
    public string SealDesc(CardSeal seal)
    {
        switch (seal)
        {
            case CardSeal.Base:
                return "";
            case CardSeal.Funding:
                return "Funding Seal: Gain $3 when scored";
            case CardSeal.Retake:
                return "Retake Seal: Retrigger this card";
            case CardSeal.Study:
                return "Study Seal: Generates Textbook for last hand played if held in hand at end of round";
        }

        return "";
    }

    // Use enhancement effect with given use location
    public void UseEnhancement(PCard card, UseLocation location)
    {
        //  Ignore, part of check in currentHandManager or no enhancement
        if(card.enhancement == CardEnhancement.Base || card.enhancement == CardEnhancement.WildCard)
        {
            return;
        }

        if(location == UseLocation.PreCard)
        {

        }

        //  XMult before the +Mult
        if(location == UseLocation.PostCard)
        {

        }

        //  "Held in hand" enhancement effects
        if (location == UseLocation.PostFromDraw)
        {

        }
    }

    // Use edition effect with given use location
    public void UseEdition(PCard card, UseLocation location)
    {
        //  Do nothing as they don't affect scoring
        if(card.edition == CardEdition.Base || card.edition == CardEdition.Negative)
        {
            return;
        }

        if(location == UseLocation.PreCard)
        {

        }

        //  XMult before the +Mult
        if (location == UseLocation.PostCard)
        {

        }

        //  This is for Editions that are are on Jokers
        if (location == UseLocation.Post)
        {

        }
    }

    // Use seal effect with given use location
    public void UseSeal(PCard card, UseLocation location)
    {
        //  Do nothing
        if(card.seal == CardSeal.Base)
        {
            return;
        }

        if(location == UseLocation.PreCard)
        {

        }

        if(location == UseLocation.PostFromDraw)
        {

        }
    }
}
