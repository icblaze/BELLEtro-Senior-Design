// This Document contains the code for the Game class.
// This class is used to hold information about the game's state
// Current Devs:
// Robert (momomonkeyman): made base class with the variables
// Andy (flakkid): Added class and the editionRates dictionary
// Zacharia Alaoui: Added the functions and implemented the dictionaries

using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CardModifier
{
    private static System.Random rand = new System.Random();
    private ScoringManager scoringManager;
    private ConsumableCardHolder consumableCardHolder;

    //  Adjust time of scoring manager between each score increment
    private readonly float waitIncrement = 0.5f;

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
                return "\nBonus Card: <color=blue>+30 Chips</color>";
            case CardEnhancement.MultCard:
                return "\nMult Card: <color=red>+4 Mult</color>";
            case CardEnhancement.WildCard:
                return "\nWild Card: Can be used as any suit";
            case CardEnhancement.GoldCard:
                return "\nGold Card: Gain <color=#BB8525FF>$3</color> if held in hand at end of round";
            case CardEnhancement.SteelCard:
                return "\nSteel Card: <b><color=red>X1.5 Mult</color></b> when held in hand";
            case CardEnhancement.GlassCard:
                return "\nGlass Card: <b><color=red>X2 Mult</color></b> when scored. 25% chance of being destroyed after scoring";
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
                return "\nFoil: <color=blue>+50 Chips</color>";
            case CardEdition.Holographic:
                return "\nHolographic: <color=red>+10 Mult</color>";
            case CardEdition.Polychrome:
                return "\nPolychrome: <b><color=red>X1.5 Mult</color></b>";
            case CardEdition.Negative:
                return "\nNegative: <color=purple>+1 Mentor Slot</color>";
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
                return "\nFunding Seal: Gain <color=#BB8525FF>$3</color> when scored";
            case CardSeal.Retake:
                return "\nRetake Seal: Retrigger this card";
            case CardSeal.Study:
                return "\nStudy Seal: Generates Textbook for last hand played if held in hand at end of round";
        }

        return "";
    }

    // Use enhancement effect with given use location
    public IEnumerator UseEnhancement(PCard card, UseLocation location)
    {
        if (scoringManager == null)
        {
            scoringManager = GameObject.FindFirstObjectByType<ScoringManager>();
        }

        if (card.enhancement == CardEnhancement.Base || card.enhancement == CardEnhancement.WildCard)   
        {
            yield break; //  Don't affect scoring
        }
        else if (location == UseLocation.PreCard)
        {
            if (card.enhancement == CardEnhancement.BonusCard)
            {
                scoringManager.IncrementCurrentChips(30); // +30 chips
                yield return scoringManager.ScorePopupPCard(card, $"<color=blue>+30 Chips</color>");
            }
            else if (card.enhancement == CardEnhancement.MultCard)
            {
                scoringManager.IncrementCurrentMult(4); // +4 Mult
                yield return scoringManager.ScorePopupPCard(card, $"<color=red>+4 Mult</color>");
            }
        }
        else if (location == UseLocation.PostCard)   //  XMult after the +Mult
        {
            if (card.enhancement == CardEnhancement.GlassCard)
            {
                int xmult = scoringManager.GetCurrentMult() * 2; // X2 Mult
                scoringManager.SetCurrentMult(xmult);
                yield return scoringManager.ScorePopupPCard(card, $"<b><color=red>X2 Mult</color></b>");

                //  1 in 4 chance to break
                if (Random.Range(0, 4) == 0)
                {
                    yield return scoringManager.ScorePopupPCard(card, $"<i>Crack!</i>");
                    Deck.access().MarkGlass(card.cardID);  //  Add to break stack
                }
            }
        }
        else if (location == UseLocation.PostFromDraw)  //  "Held in hand" enhancement effects
        {
            if (card.enhancement == CardEnhancement.SteelCard)
            {
                int xmult = (int) (scoringManager.GetCurrentMult() * 1.5f); // X1.5 Mult
                scoringManager.SetCurrentMult(xmult);
                yield return scoringManager.ScorePopupHeld(card, $"<b><color=red>X1.5 Mult</color></b>");
            }
        }
        else if (location == UseLocation.PostBlind)  //  "Held in hand" after round completed
        {
            if (card.enhancement == CardEnhancement.GoldCard)
            {
                Player.access().moneyCount += 3;
                yield return scoringManager.ScorePopupHeld(card, "<color=yellow>$3</color>");
                ShopManager.access().UpdateMoneyDisplay();
            }
        }
    }

    // Use edition effect with given use location
    public IEnumerator UseEdition(PCard card, UseLocation location)
    {
        if (card.edition == CardEdition.Base || card.edition == CardEdition.Negative)    
        {
            yield break; //  Don't affect scoring
        }
        else if (location == UseLocation.PreCard)
        {
            if (card.edition == CardEdition.Foil)
            {
                scoringManager.IncrementCurrentChips(50); // +50 chips
                yield return scoringManager.ScorePopupPCard(card, $"<color=blue>+50 Chips</color>");
            }
            else if (card.edition == CardEdition.Holographic)
            {
                scoringManager.IncrementCurrentMult(10); // +10 Mult
                yield return scoringManager.ScorePopupPCard(card, $"<color=red>+10 Mult</color>");
            }
        }
        else if (location == UseLocation.PostCard)  //  XMult after the +Mult
        {
            if (card.edition == CardEdition.Polychrome)
            {
                int xmult = (int)(scoringManager.GetCurrentMult() * 1.5f); // X1.5 Mult
                scoringManager.SetCurrentMult(xmult);
                yield return scoringManager.ScorePopupPCard(card, $"<b><color=red>X1.5 Mult</color></b>");
            }
        }
    }

    //  Use edition effect of the mentors from (this will be called from left to right)
    public IEnumerator UseMentorEdition(Mentor mentor, bool checkPoly)
    {
        if(mentor.edition == CardEdition.Base || mentor.edition == CardEdition.Negative)
        {
            yield break; //  Don't affect scoring
        }
        else if (!checkPoly)
        {
            if (mentor.edition == CardEdition.Foil)
            {
                scoringManager.IncrementCurrentChips(50); // +50 chips
                yield return scoringManager.ScorePopupMentor(mentor, $"<color=blue>+50 Chips</color>");
            }
            else if (mentor.edition == CardEdition.Holographic)
            {
                scoringManager.IncrementCurrentMult(10); // +10 Mult
                yield return scoringManager.ScorePopupMentor(mentor, $"<color=red>+10 Mult</color>");
            }
        }
        else if (checkPoly && mentor.edition == CardEdition.Polychrome)
        {
            int xmult = (int)(scoringManager.GetCurrentMult() * 1.5f);  // X1.5 Mult
            scoringManager.SetCurrentMult(xmult);
            yield return scoringManager.ScorePopupMentor(mentor, $"<b><color=red>X1.5 Mult</color></b>");
        }
        yield return new WaitForSecondsRealtime(waitIncrement);
    }

    // Use seal effect with given use location
    public IEnumerator UseSeal(PCard card, UseLocation location)
    {
        if(card.seal == CardSeal.Base)
        {
            yield break; //  Don't affect scoring
        }
        else if(location == UseLocation.Retrigger)
        {
            // If card has red seal, increment retrigger count for this card
            if (card.seal == CardSeal.Retake)
            {
                card.replayCounter++;
                yield break;
            }
        }
        else if(location == UseLocation.PreCard)
        {
            if(card.seal == CardSeal.Funding)
            {
                Player.access().moneyCount += 3;
                yield return scoringManager.ScorePopupPCard(card, "<color=yellow>$3</color>");
                ShopManager.access().UpdateMoneyDisplay();
            }
        }
        else if(location == UseLocation.PostBlind)  //  "Held in hand" after round completed
        {
            if (card.seal == CardSeal.Study)
            {
                if (Player.access().consumables.Count >= Player.access().maxConsumables)
                {
                    yield return null;  //  Don't generate textbook if no space
                }
                else
                {
                    if(consumableCardHolder == null)
                    {
                        consumableCardHolder = GameObject.FindFirstObjectByType<ConsumableCardHolder>();
                    }
                    yield return scoringManager.ScorePopupHeld(card, "<color=#008080ff>Studied!</color>");
                    TextbookName handTextbook = GetTextbookFromString(scoringManager.GetCurrentHandType());
                    consumableCardHolder.AddConsumable(new Textbook(handTextbook));
                }
            }
        }
    }

    public TextbookName GetTextbookFromString(string handName)
    {
        switch(handName)
        {
            case "HighCard":
                return TextbookName.HighCard;
            case "Pair":
                return TextbookName.Pair;
            case "TwoPair":
                return TextbookName.TwoPair;
            case "ThreeKind":
                return TextbookName.ThreeKind;
            case "Straight":
                return TextbookName.Straight;
            case "Flush":
                return TextbookName.Flush;
            case "FullHouse":
                return TextbookName.FullHouse;
            case "FourKind":
                return TextbookName.FourKind;
            case "StraightFlush":
                return TextbookName.StraightFlush;
            case "FiveKind":
                return TextbookName.FiveKind;
            case "FlushHouse":
                return TextbookName.FlushHouse;
            case "FlushFive":
                return TextbookName.FlushFive;
        }

        return TextbookName.HighCard;
    }
}
