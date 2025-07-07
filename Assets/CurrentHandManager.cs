//Script is used to detect the different types of hands
//that the current selected cards make.
//Current Devs:
//Fredrick Bouloute (bouloutef04) - Implemented the base logic and text changes for the Hand Types.
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

    public List<PCard> pairCards = new List<PCard>(); //List of PCards that are used to check for pairs
    public List<PCard> twoPairCards = new List<PCard>(); //List of PCards that are used to check for two pairs
    public List<PCard> threeOfAKindCards = new List<PCard>(); //List of PCards that are used to check for three of a kind
    public List<PCard> fourOfAKindCards = new List<PCard>(); //List of PCards that are used to check for four of a kind
    public List<PCard> fiveOfAKindCards = new List<PCard>(); //List of PCards that are used to check for five of a kind
    public List<PCard> fullHouseCards = new List<PCard>(); //List of PCards that are used to check for full house
    public List<PCard> flushCards = new List<PCard>(); //List of PCards that are used to check for flush    
    public List<PCard> straightCards = new List<PCard>(); //List of PCards that are used to check for straigh
    public PCard highCard; //High Card used to check for high card

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
                highCard = selectedCards[0];
                currentHandText.GetComponent<TMP_Text>().text = "High Card";
                updateBaseAndMult("HighCard");
                return "HighCard";
            case 2://If pair, return pair. If not highcard

                result = PairCheck(selectedCards);
                if (result.CompareTo("HighCard") == 0)
                {
                    currentHandText.GetComponent<TMP_Text>().text = "High Card";
                    updateBaseAndMult(result);
                    return result;
                }
                else
                {
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
            case 3://Check if pair, three of a kind, or highcard.
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "ThreeKind";
                    currentHandText.GetComponent<TMP_Text>().text = "Three Of A Kind";
                    updateBaseAndMult(result);
                    return result;
                }
                result = PairCheck(selectedCards);
                if (result.CompareTo("HighCard") == 0)
                {
                    currentHandText.GetComponent<TMP_Text>().text = "High Card";
                    updateBaseAndMult(result);
                    return result;
                }
                else
                {
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
            case 4://Check pair, three of a kind, four of a kind, two pair, highcard.
                if (FourKindCheck(selectedCards) == true)
                {
                    result = "FourKind";
                    currentHandText.GetComponent<TMP_Text>().text = "Four Of A Kind";
                    updateBaseAndMult(result);
                    return result;
                }
                if (ThreeKindCheck(selectedCards) == true)
                {
                    Debug.LogWarning("Three of a kind detected in 4 card hand!");
                    result = "ThreeKind";
                    currentHandText.GetComponent<TMP_Text>().text = "Three Of A Kind";
                    updateBaseAndMult(result);
                    return result;
                }
                if (TwoPairCheck(selectedCards) == true)
                {
                    result = "TwoPair";
                    currentHandText.GetComponent<TMP_Text>().text = "Two Pair";
                    updateBaseAndMult(result);
                    return result;
                }
                if (PairCheck(selectedCards) == "Pair")
                {
                    result = "Pair";
                    currentHandText.GetComponent<TMP_Text>().text = "Pair";
                    updateBaseAndMult(result);
                    return result;
                }
                else
                {
                    result = "HighCard";
                    currentHandText.GetComponent<TMP_Text>().text = "High Card";
                    updateBaseAndMult(result);
                    return result;
                }
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
                        result = "FlushFive";
                        currentHandText.GetComponent<TMP_Text>().text = "Flush Five";
                        updateBaseAndMult(result);
                        return result;
                    }
                    result = "FiveKind";
                    currentHandText.GetComponent<TMP_Text>().text = "Five Of A Kind";
                    updateBaseAndMult(result);
                    return result;
                }
                if (fullhouse == true && flush == true)
                {
                    result = "FlushHouse";
                    currentHandText.GetComponent<TMP_Text>().text = "Flush House";
                    updateBaseAndMult(result);
                    return result;
                }
                if (flush == true && straight == true)
                {
                    result = "StraightFlush";
                    currentHandText.GetComponent<TMP_Text>().text = "Straight Flush";
                    updateBaseAndMult(result);
                    return result;
                }
                if (FourKindCheck(selectedCards) == true)
                {
                    result = "FourKind";
                    currentHandText.GetComponent<TMP_Text>().text = "Four Of A Kind";
                    updateBaseAndMult(result);
                    return result;
                }
                if (fullhouse == true)
                {
                    result = "FullHouse";
                    currentHandText.GetComponent<TMP_Text>().text = "Full House";
                    updateBaseAndMult(result);
                    return result;
                }
                if (flush == true)
                {
                    result = "Flush";
                    currentHandText.GetComponent<TMP_Text>().text = "Flush";
                    updateBaseAndMult(result);
                    return result;
                }
                if (straight == true)
                {
                    result = "Straight";
                    currentHandText.GetComponent<TMP_Text>().text = "Straight";
                    updateBaseAndMult(result);
                    return result;
                }
                if (ThreeKindCheck(selectedCards) == true)
                {
                    result = "ThreeKind";
                    currentHandText.GetComponent<TMP_Text>().text = "Three Of A Kind";
                    updateBaseAndMult(result);
                    return result;
                }
                if (TwoPairCheck(selectedCards) == true)
                {
                    result = "TwoPair";
                    currentHandText.GetComponent<TMP_Text>().text = "Two Pair";
                    updateBaseAndMult(result);
                    return result;
                }
                result = PairCheck(selectedCards);
                if (result.CompareTo("HighCard") == 0)
                {
                    currentHandText.GetComponent<TMP_Text>().text = "High Card";
                    updateBaseAndMult(result);
                    return result;
                }
                else
                {
                    currentHandText.GetComponent<TMP_Text>().text = result;
                    updateBaseAndMult(result);
                    return result;
                }
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
        pairCards.Clear(); //Clear the pair cards list before checking for pairs

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
                        //Found a vowel pair
                        pairCards.Add(pCards[i]);
                        pairCards.Add(pCards[j]);
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
                    pairCards.Add(pCards[i]);
                    pairCards.Add(pCards[j]);
                    Debug.LogWarning("Pair with same manner of articulation!");
                    return "Pair";
                }
                //Comparing the place of articulation of the 2 different Lingustic Term phrases
                else if (cardTerms[i][2] == cardTerms[j][2])
                {
                    pairCards.Add(pCards[i]);
                    pairCards.Add(pCards[j]);
                    Debug.LogWarning("Pair with same place of articulation!");
                    return "Pair";
                }
            }
        }

        return "HighCard";
    }

    public bool ThreeKindCheck(List<PCard> pCards)
    {
        if (pCards.Count < 3)
        {
            return false;
        }

        string[][] cardTerms = new string[pCards.Count][];
        threeOfAKindCards.Clear(); //Clear the three of a kind cards list before checking for three of a kind

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
                            //Found a vowel three of a kind
                            threeOfAKindCards.Add(pCards[i]);
                            threeOfAKindCards.Add(pCards[j]);
                            threeOfAKindCards.Add(pCards[k]);
                            Debug.LogWarning("Three Of A kind with vowels!");
                            return true;
                        }
                        continue;
                    }
                    //Comparing the manner of articulation of the 3 Lingustic Term phrases
                    if ((cardTerms[i][1] == cardTerms[j][1]) && (cardTerms[i][1] == cardTerms[k][1]))
                    {
                        threeOfAKindCards.Add(pCards[i]);
                        threeOfAKindCards.Add(pCards[j]);
                        threeOfAKindCards.Add(pCards[k]);
                        Debug.LogWarning("Three Of A kind with consonants, same manner of articulation amongst the terms!");
                        return true;
                    }
                    //Comparing the place of articulation of the 3 Lingustic Term phrases
                    else if ((cardTerms[i][2] == cardTerms[j][2]) && (cardTerms[i][2] == cardTerms[k][2]))
                    {
                        threeOfAKindCards.Add(pCards[i]);
                        threeOfAKindCards.Add(pCards[j]);
                        threeOfAKindCards.Add(pCards[k]);
                        //Found a consonant three of a kind
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
        twoPairCards.Clear(); //Clear the two pair cards list before checking for two pairs

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
                        twoPairCards.Add(pCards[i]);
                        twoPairCards.Add(pCards[j]);
                        usedIndexes.Add(i);
                        usedIndexes.Add(j);
                        //Found a vowel pair
                        Debug.Log("Vowel Pair inside the TwoPairCheck!");
                        pairCount++;
                        break;
                    }

                }
                //Comparing the manner of articulation of the 2 different Lingustic Term phrases
                else if (cardTerms[i][1] == cardTerms[j][1])
                {
                    twoPairCards.Add(pCards[i]);
                    twoPairCards.Add(pCards[j]);
                    usedIndexes.Add(i);
                    usedIndexes.Add(j);
                    Debug.LogWarning("Pair with same manner of articulation!");
                    pairCount++;
                    break;
                }
                //Comparing the place of articulation of the 2 different Lingustic Term phrases
                else if (cardTerms[i][2] == cardTerms[j][2])
                {
                    twoPairCards.Add(pCards[i]);
                    twoPairCards.Add(pCards[j]);
                    usedIndexes.Add(i);
                    usedIndexes.Add(j);
                    Debug.LogWarning("Pair with same place of articulation!");
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


        if (cardsPlaceOfArt && cardsPlaceOfManner)
        {
            for (int i = 0; i < cardTerms.Length; i++)
            {
                fourOfAKindCards.Clear(); // Clear the four of a kind cards list before checking for four of a kind
                fourOfAKindCards.Add(pCards[i]); // Add the first card to the four of a kind cards list
                for (int j = i + 1; j < cardTerms.Length; j++)
                {
                    if (cardTerms[i][1] == cardTerms[j][1] && cardTerms[i][2] == cardTerms[j][2])
                    {
                        fourOfAKindCards.Add(pCards[j]);
                    }
                }

                if (fourOfAKindCards.Count == 4)
                {
                    Debug.LogWarning("Four of a Kind with vowels!");
                    return true; // Found a four of a kind
                }
            }
        }

        //This checks if the consonants have the same manner of articulation
        if (cardsPlaceOfManner && !cardsPlaceOfArt)
        {
            for (int i = 0; i < cardTerms.Length; i++)
            {
                fourOfAKindCards.Clear(); // Clear the four of a kind cards list before checking for four of a kind
                fourOfAKindCards.Add(pCards[i]); // Add the first card to the four of a kind cards list
                for (int j = i + 1; j < cardTerms.Length; j++)
                {
                    if (cardTerms[i][1] == cardTerms[j][1])
                    {
                        fourOfAKindCards.Add(pCards[j]);
                    }
                }

                if (fourOfAKindCards.Count == 4)
                {
                    Debug.LogWarning("Four of a Kind with same manner of articulation detected!");
                    return true; // Found a four of a kind
                }
            }
        }

        //This checks if the consonants have the same place of articulation
        if (cardsPlaceOfArt)
        {
            for (int i = 0; i < cardTerms.Length; i++)
            {
                fourOfAKindCards.Clear(); // Clear the four of a kind cards list before checking for four of a kind
                fourOfAKindCards.Add(pCards[i]); // Add the first card to the four of a kind cards list
                for (int j = i + 1; j < cardTerms.Length; j++)
                {
                    if (cardTerms[i][2] == cardTerms[j][2])
                    {
                        fourOfAKindCards.Add(pCards[j]);
                    }
                }

                if (fourOfAKindCards.Count == 4)
                {
                    Debug.LogWarning("Four of a Kind with same place of articulation detected!");
                    return true; // Found a four of a kind
                }
            }
        }

        Debug.LogWarning("Four of a Kind not detected!");
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
        straightCards.Clear(); //Clear the straight cards list before checking for a straight

        //This checks if the vowels have the same place of articulation and manner of articulation
        for (int i = 0; i < pCards.Count; i++)
        {
            string suit = pCards[i].suit.ToString();
            string place = pCards[i].placeArt.ToString();
            string manner = pCards[i].mannerArt.ToString();

            bool isVowel = suit == "Lax" || suit == "Tense";

            if (isVowel)
            {
                string combo = place + "|" + manner;
                if (!vowelPairs.Add(combo))
                    return false; // Duplicate vowel combo
            }
        }

        if (vowelPairs.Count == 5)
        {
            straightCards.AddRange(pCards.Where(card => card.suit == SuitName.Lax || card.suit == SuitName.Tense));
            // All vowels are in a straight
            Debug.LogWarning("Straight with Vowels detected!");
            return true;
        }

        //This checks if the consonants have the same place of articulation
        for (int i = 0; i < pCards.Count; i++)
        {
            string place = pCards[i].placeArt.ToString();
            //string manner = pCards[i].mannerArt.ToString();

            string combo = place;
            //Check if the combo is a place of articulation
            if (isAPlaceOfArticulation(place))
            {
                if (!consonantPlaces.Add(combo))
                {
                    return false; // Duplicate consonant combo
                }
            }
            else
            {
                return false; // If any non-place of articulation is present, we cannot have a straight with consonants
            }
        }

        if (consonantPlaces.Count == 5)
        {
            straightCards.AddRange(pCards.Where(card => isAPlaceOfArticulation(card.placeArt.ToString())));
            // All consonants are in a straight
            Debug.LogWarning("Straight with Consonants detected!");
            return true;
        }

        // Mixed or insufficient cards for a straight
        Debug.LogWarning("No Straight detected!");
        return false;
    }


    //This checks if all the suits are the same among the selected cards that have been chosen by the user.
    public bool FlushCheck(List<PCard> pCards)
    {
        if (pCards.Count < 5)
        {
            return false;
        }

        string[][] cardTerms = new string[pCards.Count][];
        flushCards.Clear(); //Clear the flush cards list before checking for a flush

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }

        for (int i = 0; i < cardTerms.Length; i++)
        {
            if (cardTerms[i] == null)
                Debug.LogError($"cardTerms[{i}] is null!");
            else if (cardTerms[i].Length == 0)
                Debug.LogError($"cardTerms[{i}] is empty!");
        }



        bool isFlush = cardTerms.GroupBy(term => term[0]).Any(g => g.Count() >= 5);


        if (isFlush)
        {
            // Add all cards with the same suit to the flushCards list
            foreach (var card in pCards)
            {
                flushCards.Add(card);
            }
            Debug.LogWarning("Flush detected!");
            return true; // Found a flush
        }
        else
        {
            Debug.LogWarning("Flush not detected!");
        }

        return false; // No flush found
    }

    //Find a pair. Then check to see if the remaining cards are
    //a three of a kind.
    public bool FullHouseCheck(List<PCard> pCards)
    {
        if (pCards == null || pCards.Count < 5)
        {
            Debug.LogWarning("FullHouseCheck: Invalid card list provided.");
            return false;
        }


        List<PCard> listOfCards = pCards;
        string[][] cardTerms = new string[pCards.Count][];
        HashSet<int> usedIndexes = new HashSet<int>(); // Used to track the indexes
        bool isThreeKind = false;
        fullHouseCards.Clear(); //Clear the full house cards list before checking for a full house

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
                    if ((pCards[i].suit == SuitName.Lax || pCards[i].suit == SuitName.Tense) &&
                        (pCards[j].suit == SuitName.Lax || pCards[j].suit == SuitName.Tense) &&
                        (pCards[k].suit == SuitName.Lax || pCards[k].suit == SuitName.Tense))
                    {
                        if ((pCards[i].placeArt == pCards[j].placeArt) &&
                            (pCards[i].placeArt == pCards[k].placeArt) &&
                            (pCards[i].mannerArt == pCards[j].mannerArt) &&
                            (pCards[i].mannerArt == pCards[k].mannerArt))
                        {
                            //Found a vowel three of a kind
                            fullHouseCards.Add(pCards[i]);
                            fullHouseCards.Add(pCards[j]);
                            fullHouseCards.Add(pCards[k]);

                            //Add the indexes to the usedIndexes set    
                            usedIndexes.Add(i);
                            usedIndexes.Add(j);
                            usedIndexes.Add(k);

                            Debug.LogWarning("Potential Full house!");
                            isThreeKind = true;
                            break;
                        }
                    }
                    //Comparing the manner of articulation of the 3 Lingustic Term phrases
                    else if ((pCards[i].mannerArt == pCards[j].mannerArt) && (pCards[i].mannerArt == pCards[k].mannerArt))
                    {
                        fullHouseCards.Add(pCards[i]);
                        fullHouseCards.Add(pCards[j]);
                        fullHouseCards.Add(pCards[k]);

                        usedIndexes.Add(i);
                        usedIndexes.Add(j);
                        usedIndexes.Add(k);
                        Debug.LogWarning("Potential Full house!");
                        isThreeKind = true;
                        break;
                    }
                    //Comparing the place of articulation of the 3 Lingustic Term phrases
                    else if ((pCards[i].placeArt == pCards[j].placeArt) && (pCards[i].placeArt == pCards[k].placeArt))
                    {
                        fullHouseCards.Add(pCards[i]);
                        fullHouseCards.Add(pCards[j]);
                        fullHouseCards.Add(pCards[k]);

                        //Found a consonant three of a kind
                        usedIndexes.Add(i);
                        usedIndexes.Add(j);
                        usedIndexes.Add(k);

                        Debug.LogWarning("Potential Full house!");
                        isThreeKind = true;
                        break;
                    }
                }
            }
        }

        //This removes all the indexes that were used to find the three of a kind from the list of cards.
        listOfCards = listOfCards.Where((card, index) => !usedIndexes.Contains(index)).ToList();

        if ((PairCheck(listOfCards) == "Pair") && isThreeKind)
        {
            fullHouseCards.AddRange(listOfCards);
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
        fiveOfAKindCards.Clear(); //Clear the five of a kind cards list before checking for five of a kind

        //Convert the Enum lingustic terms into a string
        for (int i = 0; i < pCards.Count; i++)
        {
            string formatted = SplitCase.Split(pCards[i].term.ToString());
            cardTerms[i] = formatted.Split(' ');
        }


        //These variable groups all the cards by having the same manner or place of articulation, and returns true if
        //there is 5 cards that have the same manner of articulation or place of articulation
        bool cardsPlaceOfManner = cardTerms.GroupBy(cardTerm => cardTerm[1]).Any(g => g.Count() == 5);
        bool cardsPlaceOfArt = cardTerms.GroupBy(cardTerm => cardTerm[2]).Any(g => g.Count() == 5);

        if (cardsPlaceOfArt && cardsPlaceOfManner)
        {
            fiveOfAKindCards.AddRange(pCards);
            Debug.LogWarning("Five of a Kind detected for vowels!");
            return true;
        }

        if (cardsPlaceOfArt || cardsPlaceOfManner)
        {

            fiveOfAKindCards.AddRange(pCards);
            Debug.LogWarning("Five of a Kind detected for consonants!");
            return true;
        }

        return false;
    }

    public PCard GetHighCard(List<PCard> selectedCards)
    {
        //Check if the selected cards are null or empty
        if (selectedCards == null || selectedCards.Count == 0)
        {
            Debug.LogWarning("Selected cards list is null or empty.");
            return null;
        }

        //Select the first card as the high card
        //Might need to change this logic so only the highest card is selected
        highCard = selectedCards[0];

        //Debug log to show the high card found
        Debug.Log("High Card Found: " + highCard.term.ToString());

        return highCard;
    }

    public List<PCard> GetListOfCards(string handType)
    {
        //Returns the list of cards based on the hand type
        switch (handType)
        {
            case "Pair":
                return pairCards;
            case "TwoPair":
                return twoPairCards;
            case "ThreeKind":
                return threeOfAKindCards;
            case "Straight":
                return straightCards;
            case "Flush":
                return flushCards;
            case "FullHouse":
                return fullHouseCards;
            case "FourKind":
                return fourOfAKindCards;
            case "StraightFlush":
                return straightCards;
            case "FiveKind":
                return fiveOfAKindCards;
            case "FlushHouse":
                return flushCards;
            case "FlushFive":
                return fiveOfAKindCards;
            default:
                Debug.LogWarning("Invalid hand type: " + handType);
                return new List<PCard>();
        }
    }

    //This function updates the the blue chip and red chip text fields in the round UI based on the seleceted hand
    public void updateBaseAndMult(string handType)
    {
        switch (handType)
        {
            case "HighCard":
                blueScoreText.text = Player.access().handTable[TextbookName.HighCard].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.HighCard].GetCurrMult().ToString();
                break;
            case "Pair":
                blueScoreText.text = Player.access().handTable[TextbookName.Pair].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.Pair].GetCurrMult().ToString();
                break;
            case "TwoPair":
                blueScoreText.text = Player.access().handTable[TextbookName.TwoPair].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.TwoPair].GetCurrMult().ToString();
                break;
            case "ThreeKind":
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
            case "FullHouse":
                blueScoreText.text = Player.access().handTable[TextbookName.FullHouse].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FullHouse].GetCurrMult().ToString();
                break;
            case "FourKind":
                blueScoreText.text = Player.access().handTable[TextbookName.FourKind].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FourKind].GetCurrMult().ToString();
                break;
            case "StraightFlush":
                blueScoreText.text = Player.access().handTable[TextbookName.StraightFlush].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.StraightFlush].GetCurrMult().ToString();
                break;
            case "FiveKind":
                blueScoreText.text = Player.access().handTable[TextbookName.FiveKind].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FiveKind].GetCurrMult().ToString();
                break;
            case "FlushHouse":
                blueScoreText.text = Player.access().handTable[TextbookName.FlushHouse].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FlushHouse].GetCurrMult().ToString();
                break;
            case "FlushFive":
                blueScoreText.text = Player.access().handTable[TextbookName.FlushFive].GetCurrChips().ToString();
                redScoreText.text = Player.access().handTable[TextbookName.FlushFive].GetCurrMult().ToString();
                break;
            default:
                blueScoreText.text = "0";
                redScoreText.text = "0";
                return;
        }
    }

    public bool isAPlaceOfArticulation(string place)
    {
        // Check if the place is one of the valid places of articulation
        return place == "Labial" || place == "Labiodental" || place == "Interdental" ||
               place == "Alveolar" || place == "AlveoPalatal" || place == "Velar" ||
               place == "Glottal";
    }

}
