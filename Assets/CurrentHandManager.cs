//Script is used to detect the different types of hands
//that the current selected cards make.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
//Zacharia Alaoui (ZachariaAlaoui) - Implemented the logic for detecting and updating the UI based on the hand the player currently has selected.

using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using SplitString;
using NUnit.Framework.Interfaces;

public class CurrentHandManager : MonoBehaviour
{
    private bool fullhouse;
    private bool straight;
    private bool flush;
    private bool fiveOfAKind;
    private GameObject currentHandText;
    private GameObject blueScore;
    private GameObject redScore;
    private HandInfo handInfo;
    private TMP_Text blueScoreText;
    private TMP_Text redScoreText;

    public static CurrentHandManager Instance { get; private set; }  //Player instance varaiable

    void Awake()
    {
        // Enforce singleton instance
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    void Start()
    {
        currentHandText = GameObject.Find("Current Hand Text");
        currentHandText.GetComponent<TMP_Text>().text = "";

        blueScore = GameObject.Find("Blue Score Text");
        blueScoreText = blueScore.GetComponent<TMP_Text>();
        blueScoreText.text = "0";

        redScore = GameObject.Find("Red Score Text");
        redScoreText = redScore.GetComponent<TMP_Text>();
        redScoreText.text = "0";
    }

    //Function is used to detect the current hand. When called,
    //the current hand of PCards will be given and it will 
    //use the count to go through different tests for 
    //each hand.
    public string findCurrentHand(List<PCard> selectedCards)
    {
        if (selectedCards == null)
        {
            Debug.LogWarning("The list passed in is not initialized and is null!");
            return "";
        }

        string result;
        switch (selectedCards.Count)
        {
            case 0:
                currentHandText.GetComponent<TMP_Text>().text = "";
                blueScoreText.text = "0";
                redScoreText.text = "0";
                return "";
            case 1:
                currentHandText.GetComponent<TMP_Text>().text = "High Card";
                updateBaseAndMult("High Card");
                return "HighCard";
            case 2://If pair, return pair. If not highcard

                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                updateBaseAndMult(result);
                return result;
            case 3://Check if pair, three of a kind, or highcard.
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "Three Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                updateBaseAndMult(result);
                return result;
            case 4://Check pair, three of a kind, four of a kind, two pair, highcard.
                if (FourKindCheck(selectedCards) == true)
                {
                    result = "Four Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "Three Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (TwoPairCheck(selectedCards) == true)
                {
                    result = "Two Pair";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                updateBaseAndMult(result);
                return result;
            case 5:
                //Do all Five Card functions to test if they are flushes
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
                        updateBaseAndMult(result);
                        return result;
                    }
                    result = "Five Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (fullhouse == true && flush == true)
                {
                    result = "Flush House";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (flush == true && straight == true)
                {
                    result = "Straight Flush";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (FourKindCheck(selectedCards) == true)
                {
                    result = "Four Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (fullhouse == true)
                {
                    result = "Full House";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (flush == true)
                {
                    result = "Flush";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (straight == true)
                {
                    result = "Straight";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "Three Of A Kind";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                if (TwoPairCheck(selectedCards) == true)
                {
                    result = "Two Pair";
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
                result = PairCheck(selectedCards);
                currentHandText.GetComponent<TMP_Text>().text = result;
                updateBaseAndMult(result);
                return result;
            default:
                Debug.LogWarning("Something went wrong. Card Size: " + selectedCards.Count);
                result = "";
                currentHandText.GetComponent<TMP_Text>().text = result;
                updateBaseAndMult(result);
                return result;
        }
    }

    //Loop through cards to check if a pair exists
    public string PairCheck(List<PCard> pCards)
    {
        string[][] cardTerms = new string[pCards.Count][];       //This will hold all the card linguistic term names in a string format.

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        //Check to see if two cards have the same place of articulation, the index for the word in the string should be 2
        //Then check to see if the two cards played have the same manner of articulation, the index should be 1
        for (int i = 0; i < cardTerms.Length - 1; i++)
        {
            for (int j = i + 1; j < cardTerms.Length; j++)
            {
                //In order to have a pair with vowels they must have the same height and placement
                if ((cardTerms[i][0] == "Lax" || cardTerms[i][0] == "Tense") && (cardTerms[j][0] == "Lax" || cardTerms[j][0] == "Tense"))
                {
                    if ((cardTerms[i][1] == cardTerms[j][1]) && (cardTerms[i][2] == cardTerms[j][2]))
                    {
                        Debug.LogWarning("Pair using vowels!");
                        return "Pair";
                    }
                    else
                    {
                        continue;
                    }
                }
                //Comparing the manner of articulation of the 2 different Lingustic Term phrases
                if (cardTerms[i][1] == cardTerms[j][1])
                {
                    Debug.LogWarning("Pair with same manner of articulation!");
                    return "Pair";
                }
                //Comparing the place of articulation of the 2 different Lingustic Term phrases
                else if (cardTerms[i][2] == cardTerms[j][2])
                {
                    Debug.LogWarning("Pair with same place of articulation!");
                    return "Pair";
                }
            }
        }

        return "High Card";
    }

    public bool ThreeKindCheck(List<PCard> pCards)
    {
        if (pCards.Count < 3)
        {
            return false;
        }

        string[][] cardTerms = new string[pCards.Count][];

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        for (int i = 0; i < cardTerms.Length - 2; i++)
        {
            for (int j = i + 1; j < cardTerms.Length - 1; j++)
            {
                for (int k = j + 1; k < cardTerms.Length; k++)
                {
                    //In order to have a three of a kind with vowels they must all have the same height and placement
                    if ((cardTerms[i][0] == "Lax" || cardTerms[i][0] == "Tense") &&
                        (cardTerms[j][0] == "Lax" || cardTerms[j][0] == "Tense") &&
                        (cardTerms[k][0] == "Lax" || cardTerms[k][0] == "Tense"))
                    {
                        if ((cardTerms[i][1] == cardTerms[j][1]) &&
                            (cardTerms[i][1] == cardTerms[k][1]) &&
                            (cardTerms[i][2] == cardTerms[j][2]) &&
                            (cardTerms[i][2] == cardTerms[k][2]))
                        {
                            Debug.LogWarning("Three Of A kind with vowels!");
                            return true;
                        }
                        continue;
                    }
                    //Comparing the manner of articulation of the 3 Lingustic Term phrases
                    if ((cardTerms[i][1] == cardTerms[j][1]) && (cardTerms[i][1] == cardTerms[k][1]))
                    {
                        Debug.LogWarning("Three Of A kind with consonants, same manner of articulation amongst the terms!");
                        return true;
                    }
                    //Comparing the place of articulation of the 3 Lingustic Term phrases
                    else if ((cardTerms[i][2] == cardTerms[j][2]) && (cardTerms[i][2] == cardTerms[k][2]))
                    {
                        Debug.LogWarning("Three Of A kind with consonants, same place of articulation amongst the terms!");
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
        int pairCount = 0;
        string[][] cardTerms = new string[pCards.Count][];
        HashSet<int> usedIndexes = new(); //Used to track the indexes where we found the pairs

        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        //Find first pair, loop through cards
        for (int i = 0; i < pCards.Count - 1; i++)
        {
            if (usedIndexes.Contains(i)) continue;

            for (int j = i + 1; j < pCards.Count; j++)
            {
                if (usedIndexes.Contains(j)) continue;

                //Check if the selected cards contain a pair of vowels
                if ((cardTerms[i][0] == "Lax" || cardTerms[i][0] == "Tense") &&
                    (cardTerms[j][0] == "Lax" || cardTerms[j][0] == "Tense"))
                {
                    if ((cardTerms[i][1] == cardTerms[j][1]) &&
                        (cardTerms[i][2] == cardTerms[j][2]))
                    {
                        //Found a vowel pair
                        Debug.Log("Vowel Pair inside the TwoPairCheck!");
                        usedIndexes.Add(i);
                        usedIndexes.Add(j);
                        pairCount++;
                        break;
                    }

                }
                //Comparing the manner of articulation of the 2 different Lingustic Term phrases
                else if (cardTerms[i][1] == cardTerms[j][1])
                {
                    Debug.LogWarning("Pair with same manner of articulation!");
                    usedIndexes.Add(i);
                    usedIndexes.Add(j);
                    pairCount++;
                    break;
                }
                //Comparing the place of articulation of the 2 different Lingustic Term phrases
                else if (cardTerms[i][2] == cardTerms[j][2])
                {
                    Debug.LogWarning("Pair with same place of articulation!");
                    usedIndexes.Add(i);
                    usedIndexes.Add(j);
                    pairCount++;
                    break;
                }
            }
        }


        return pairCount == 2;
    }
    //Check if four cards share the same term.
    public bool FourKindCheck(List<PCard> pCards)
    {
        if (pCards.Count < 4)
        {
            return false;
        }

        string[][] cardTerms = new string[pCards.Count][];

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        //These variable groups all the cards by having the same manner or place of articulation, and returns true if
        //there is 4 cards that have the same manner of articulation or place of articulation
        bool cardsPlaceOfManner = cardTerms.GroupBy(cardTerm => cardTerm[1]).Any(g => g.Count() == 4);
        bool cardsPlaceOfArt = cardTerms.GroupBy(cardTerm => cardTerm[2]).Any(g => g.Count() == 4);

        if (cardsPlaceOfArt || cardsPlaceOfManner)
        {
            return true;
        }

        return false;
    }

    //This function determines if the player has a straight in their hand
    public bool StraightCheck(List<PCard> pCards)
    {
        if (pCards.Count < 5)
        {
            return false;
        }

        var vowelPairs = new HashSet<string>();
        var consonantPlaces = new HashSet<string>();

        foreach (var card in pCards)
        {
            string suit = card.suit.ToString();
            string place = card.placeArt.ToString();
            string manner = card.mannerArt.ToString();

            bool isVowel = suit == "Lax" || suit == "Tense";

            if (isVowel)
            {
                string combo = place + "|" + manner;
                if (!vowelPairs.Add(combo))
                    return false; // Duplicate vowel combo
            }
            else
            {
                if (!consonantPlaces.Add(place))
                    return false; // Duplicate consonant place
            }
        }

        return true;
    }


    //This checks if all the suits are the same among the selected cards that have been chosen by the user.
    public bool FlushCheck(List<PCard> pCards)
    {
        if (pCards.Count < 5)
        {
            return false; 
        }

        string[][] cardTerms = new string[pCards.Count][];

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        return cardTerms.GroupBy(cardTerm => cardTerm[0]).Any(g => g.Count() == 5);
    }
    //Find a pair. Then check to see if the remaining cards are
    //a three of a kind.
    public bool FullHouseCheck(List<PCard> pCards)
    {

        List<PCard> listOfCards = pCards;
        string[][] cardTerms = new string[pCards.Count][];
        bool isThreeKind = false;

        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        for (int i = 0; i < cardTerms.Length - 2; i++)
        {
            for (int j = i + 1; j < cardTerms.Length - 1; j++)
            {
                for (int k = j + 1; k < cardTerms.Length; k++)
                {
                    //In order to have a three of a kind with vowels they must all have the same height and placement
                    if ((cardTerms[i][0] == "Lax" || cardTerms[i][0] == "Tense") &&
                        (cardTerms[j][0] == "Lax" || cardTerms[j][0] == "Tense") &&
                        (cardTerms[k][0] == "Lax" || cardTerms[k][0] == "Tense"))
                    {
                        if ((cardTerms[i][1] == cardTerms[j][1]) &&
                            (cardTerms[i][1] == cardTerms[k][1]) &&
                            (cardTerms[i][2] == cardTerms[j][2]) &&
                            (cardTerms[i][2] == cardTerms[k][2]))
                        {
                            listOfCards.RemoveAt(i);
                            listOfCards.RemoveAt(j-1);
                            listOfCards.RemoveAt(k-2);

                            Debug.LogWarning("Potential Full house!");
                            isThreeKind = true;
                            break;
                        }
                        continue;
                    }
                    //Comparing the manner of articulation of the 3 Lingustic Term phrases
                    else if ((cardTerms[i][1] == cardTerms[j][1]) && (cardTerms[i][1] == cardTerms[k][1]))
                    {
                        listOfCards.RemoveAt(i);
                        listOfCards.RemoveAt(j-1);
                        listOfCards.RemoveAt(k-2);
                        Debug.LogWarning("Potential Full house!");
                        isThreeKind = true;
                        break;
                    }
                    //Comparing the place of articulation of the 3 Lingustic Term phrases
                    else if ((cardTerms[i][2] == cardTerms[j][2]) && (cardTerms[i][2] == cardTerms[k][2]))
                    {
                        listOfCards.RemoveAt(i);
                        listOfCards.RemoveAt(j-1);
                        listOfCards.RemoveAt(k-2);
                        Debug.LogWarning("Potential Full house!");
                        isThreeKind = true;
                        break;
                    }
                }
            }
        }

        if ((PairCheck(listOfCards) == "Pair") && isThreeKind)
        {
            Debug.LogWarning("Full house!");
            return true;
        }


        return false;
    }
    //Check to see if each card shares the same term.
    public bool FiveKindCheck(List<PCard> pCards)
    {
        //Check if all five cards of the same term
        if (pCards.Count < 5)
        {
            return false;
        }

        string[][] cardTerms = new string[pCards.Count][];

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        //This loop is used to make sure that we ignore all vowels, since we cannot get a five of a kind with vowels.
        for (int i = 0; i < pCards.Count; i++)
        {
            if (cardTerms[i][0] == "Lax" || cardTerms[i][0] == "Tense")
            {
                return false;
            }
        }

        //These variable groups all the cards by having the same manner or place of articulation, and returns true if
        //there is 5 cards that have the same manner of articulation or place of articulation
        bool cardsPlaceOfManner = cardTerms.GroupBy(cardTerm => cardTerm[1]).Any(g => g.Count() == 5);
        bool cardsPlaceOfArt = cardTerms.GroupBy(cardTerm => cardTerm[2]).Any(g => g.Count() == 5);

        if (cardsPlaceOfArt || cardsPlaceOfManner)
        {
            return true;
        }

        return false;
    }

    //This function updates the the blue chip and red chip text fields in the round UI based on the seleceted hand
    public void updateBaseAndMult(string handType)
    {
        switch (handType)
        {
            case "High Card":
                blueScoreText.text = Player.access().handTable[TextbookName.HighCard].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.HighCard].GetCurrMult().ToString();
                break;
            case "Pair":
                blueScoreText.text = Player.access().handTable[TextbookName.Pair].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.Pair].GetCurrMult().ToString();
                break;
            case "Two Pair":
                blueScoreText.text = Player.access().handTable[TextbookName.TwoPair].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.TwoPair].GetCurrMult().ToString();
                break;
            case "Three Of A Kind":
                blueScoreText.text = Player.access().handTable[TextbookName.ThreeKind].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.ThreeKind].GetCurrMult().ToString();
                break;
            case "Straight":
                blueScoreText.text = Player.access().handTable[TextbookName.Straight].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.Straight].GetCurrMult().ToString();
                break;
            case "Flush":
                blueScoreText.text = Player.access().handTable[TextbookName.Flush].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.Flush].GetCurrMult().ToString();
                break;
            case "Full House":
                blueScoreText.text = Player.access().handTable[TextbookName.FullHouse].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FullHouse].GetCurrMult().ToString();
                break;
            case "Four Of A Kind":
                blueScoreText.text = Player.access().handTable[TextbookName.FourKind].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FourKind].GetCurrMult().ToString();
                break;
            case "Straight Flush":
                blueScoreText.text = Player.access().handTable[TextbookName.StraightFlush].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.StraightFlush].GetCurrMult().ToString();
                break;
            case "Five Of A Kind":
                blueScoreText.text = Player.access().handTable[TextbookName.FiveKind].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FiveKind].GetCurrMult().ToString();
                break;
            case "Flush House":
                blueScoreText.text = Player.access().handTable[TextbookName.FlushHouse].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FlushHouse].GetCurrMult().ToString();
                break;
            case "Flush Five":
                blueScoreText.text = Player.access().handTable[TextbookName.FlushFive].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FlushFive].GetCurrMult().ToString();
                break;
            default:
                blueScoreText.text = "0";
                redScoreText.text = "0";
                return;
        }
    }

}
