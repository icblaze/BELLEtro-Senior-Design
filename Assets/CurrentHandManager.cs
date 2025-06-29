using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

//Script is used to detect the different types of hands
//that the current selected cards make.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

public class CurrentHandManager : MonoBehaviour
{
    bool fullhouse;
    bool straight;
    bool flush;
    bool fiveOfAKind;
    public GameObject currentHandText;
    public void Start()
    {
        currentHandText = GameObject.Find("Current Hand Text");
    }

    //Function is used to detect the current hand. When called,
    //the current hand of PCards will be given and it will 
    //use the count to go through different tests for 
    //each hand.
    public string findCurrentHand(List<PCard> selectedCards)
    {
        string result;
        switch (selectedCards.Count)
        {
            case 0:
                currentHandText.GetComponent<TMP_Text>().text = "";
                return "";
            case 1:
                currentHandText.GetComponent<TMP_Text>().text = "High Card";
                return "HighCard";
            case 2://If pair, return pair. If not highcard

                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                return result;
            case 3://Check if pair, three of a kind, or highcard.
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "Three Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                return result;
            case 4://Check pair, three of a kind, four of a kind, two pair, highcard.
                if (FourKindCheck(selectedCards) == true)
                {
                    result = "Four Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "Three Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (TwoPairCheck(selectedCards) == true)
                {
                    result = "Two Pair";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                return result;
            case 5:
                //Do all FIve Card functions to test if they are flushes
                //five of a kinds, etc.
                fiveOfAKind = FiveKindCheck(selectedCards);
                fullhouse = FullHouseCheck(selectedCards);
                flush = FlushCheck(selectedCards);
                straight = StraightCheck(selectedCards);
                //Group the different booleans to place them into the correct hand type
                if (fiveOfAKind == true)
                {
                    if (flush == true)
                    {
                        result = "Flush Five";
                        currentHandText.GetComponent<TMP_Text>().text = result;
                        return result;
                    }
                    result = "Five Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (fullhouse == true && flush == true)
                {
                    result = "Flush House";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (flush == true && straight == true)
                {
                    result = "Straight Flush";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (FourKindCheck(selectedCards) == true)
                {
                    result = "Four Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (fullhouse == true)
                {
                    result = "Full House";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (flush == true)
                {
                    result = "Flush";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (straight == true)
                {
                    result = "Straight";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "Three Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                if (TwoPairCheck(selectedCards) == true)
                {
                    result = "Two Pair";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    return result;
                }
                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                return result;
            default:
                Debug.LogWarning("Something went wrong. Card Size: " + selectedCards.Count);
                result = "";
                currentHandText.GetComponent<TMP_Text>().text = result;
                return result;
        }
    }
    //Loop through cards to check if a pair exists
    public string PairCheck(List<PCard> pCards)
    {
        for (int i = 0; i < pCards.Count - 1; i++)
        {
            for (int j = i + 1; j < pCards.Count; j++)
            {
                if (pCards[i].term == pCards[j].term)
                {
                    return "Pair";
                }
            }
        }
        return "HighCard";
    }
    //Loops through the pCards to find a match of three
    //Big(O) is probably horrendous
    public bool ThreeKindCheck(List<PCard> pCards)
    {

        for (int i = 0; i < pCards.Count; i++)
        {
            for (int j = i + 1; j < pCards.Count; j++)
            {
                for (int k = j + 1; k < pCards.Count; k++)
                {
                    if (pCards[i].term == pCards[j].term && pCards[i].term == pCards[k].term)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    //Four Cards
    //Check if two distinct pairs exists.
    public bool TwoPairCheck(List<PCard> pCards)
    {
        int firstPairCard1 = 100;//Used to break if pair not found
        int firstPairCard2 = 0;
        //Find first pair, loop through cards
        for (int i = 0; i < pCards.Count - 1; i++)
        {
            for (int j = i + 1; j < pCards.Count; j++)
            {
                if (pCards[i].term == pCards[j].term)//Set the first two cards in pair
                {
                    firstPairCard1 = i;
                    firstPairCard2 = j;
                }
            }
        }
        if (firstPairCard1 == 100)
        {
            return false;
        }
        //Find second pair
        for (int i = 0; i < pCards.Count - 1; i++)
        {
            if (i == firstPairCard1)//If card is one of the first pairs, ignore
            {
                continue;
            }
            for (int j = i + 1; j < pCards.Count; j++)
            {
                if (j == firstPairCard2)//If card is one of the first pairs, ignore
                {
                    continue;
                }
                if (pCards[i].term == pCards[j].term)//If second pair found, return TwoPair
                {
                    return true;
                }
            }
        }
        return false;
    }
    //Check if four cards share the same term.
    public bool FourKindCheck(List<PCard> pCards)
    {
        //The following check , checks if there are 4 cards that have the same term inside the list
        return pCards.GroupBy(card => card.term).Any(group => group.Count() == 4);
    }

    //Five cards
    //Sort array and check to see if each place of articulation 
    //differs.
    public bool StraightCheck(List<PCard> pCards)
    {
        //Sort array based on place of articulation
        List<PCard> sortedCards = pCards;
        sortedCards.Sort((s1, s2) => s1.placeArt.CompareTo(s2.placeArt));

        //Loop through cards to check if all cards have different places of articulation
        for (int i = 0; i < sortedCards.Count; i++)
        {
            for (int j = i + 1; j < sortedCards.Count; j++)
            {
                if (sortedCards[i].placeArt == sortedCards[j].placeArt)
                {
                    return false;
                }
            }
        }
        return true;
    }
    //Sort through array and check if each suit is the same.
    public bool FlushCheck(List<PCard> pCards)
    {
        //Sort array based on suits
        List<PCard> sortedCards = pCards;
        sortedCards.Sort((s1, s2) => s1.suit.CompareTo(s2.suit));

        //Loop through. If suits do not match, it is not a flush
        for (int i = 0; i < pCards.Count; i++)
            for (int j = i + 1; j < pCards.Count; j++)
            {
                {
                    if (pCards[i].suit != pCards[j].suit)
                    {
                        return false;
                    }
                }
            }
        return true;
    }
    //Find a pair. Then check to see if the remaining cards are
    //a three of a kind.
    public bool FullHouseCheck(List<PCard> pCards)
    {
        int firstPairCard1 = 100;//Used to break if pair not found
        int firstPairCard2 = 0;
        //Find first pair, loop through cards
        for (int i = 0; i < pCards.Count - 1; i++)
        {
            for (int j = i + 1; j < pCards.Count; j++)
            {
                if (pCards[i].term == pCards[j].term)//Set the first two cards in pair
                {
                    firstPairCard1 = i;
                    firstPairCard2 = j;
                }
            }
        }
        //If no pair found, return null
        if (firstPairCard1 == 100)
        {
            return false;
        }
        //Check to see if next cards are three of a kind
        List<PCard> threePCards = pCards;
        threePCards.RemoveAt(firstPairCard1);
        threePCards.RemoveAt(firstPairCard2 - 1);

        if (ThreeKindCheck(threePCards) == false)
        {
            return false;
        }

        return true;
    }
    //Check to see if each card shares the same term.
    public bool FiveKindCheck(List<PCard> pCards)
    {
        //Check if all five cards of the same term
        if (pCards[0].term == pCards[1].term && pCards[0].term == pCards[2].term
        && pCards[0].term == pCards[3].term && pCards[0].term == pCards[4].term)
        {
            return true;
        }
        return false;
    }
}
