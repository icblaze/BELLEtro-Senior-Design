//Script is used to complete the scoring aspect of the game.
//This includes the chips/mults gained from the cards in addition
//to the mentors.

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class ScoringManager : MonoBehaviour
{
    private DeleteCard deleteCardScript;                            
    private List<PCard> playedPCards;                                //List of PCards played in the hand   
    private PCard highCard;                                     //High card for the hand  
    private List<GameObject> selectedCards = new List<GameObject>(); //list of selected cards
    private CurrentHandManager currentHandManager;                   //Current hand manager to get the current hand
    private ShakeScreen shakeScreen;       //ShakeScreen instance variable
    private Player player = Player.access();                //Player instance variable
    private int currentChips;                               //Current chips for the hand
    private int currentMult;                                //Current multiplier for the hand 
    private string handType;                                //Current hand type
    private GameObject roundScore;                          //GameObject for the round score
    private GameObject blueScore;                           //GameObject for the blue score 
    private GameObject redScore;                            //GameObject for the red score  
    private TMP_Text blueScoreText;                         //Text for the blue score   
    private TMP_Text redScoreText;                          //Text for the red score
    private TMP_Text roundScoreText;                        //Text for the round score
    private BigInteger totalScore;                          //Total score for the hand   
    private BigInteger neededScore;                         //Score needed to win the round

    //  Adjust time of scoring manager between each score increment
    private readonly float waitIncrement= 0.5f;

    //  Call to MentorBufferManager
    private MentorBufferManager mentorBuffer = MentorBufferManager.access();

    public void Start()
    {
        shakeScreen = GameObject.FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();  

        //Get delete card script and do null check
        deleteCardScript = FindFirstObjectByType<DeleteCard>();

        if (deleteCardScript == null)
            Debug.LogError("PlayHand: DeleteCard script not found in scene!");

        //Get current hand Manager
        currentHandManager = FindFirstObjectByType<CurrentHandManager>();
        if (currentHandManager == null)
            Debug.LogError("PlayHand: CurrentHandManager script not found in scene!");

        //Get score UI components
        roundScore = GameObject.Find("Round Score Number");
        roundScoreText = roundScore.GetComponent<TMP_Text>();

        blueScore = GameObject.Find("Blue Score Text");
        blueScoreText = blueScore.GetComponent<TMP_Text>();

        redScore = GameObject.Find("Red Score Text");
        redScoreText = redScore.GetComponent<TMP_Text>();
    }

    //Public function to start the scoring process
    public void GetScoring()
    {
        //Get PCards and Regular Cards
        playedPCards = deleteCardScript.GetSelectedPCards();
        Debug.Log("Played P Cards Count: " + playedPCards.Count);

        selectedCards = deleteCardScript.GetSelectedCards();

        //Set heldHand list (hand that exclude played PCards)
        Deck.access().SetHeldHand(playedPCards);


        //Get current hand and scores for it
        handType = currentHandManager.findCurrentHand(playedPCards);
        SetHandScore(handType);

        //Setting ante and round values for testing purposes
        Game.access().anteValue = 1;
        Game.access().roundValueTest = 1;

        int ante = Game.access().anteValue;
        int round = Game.access().roundValueTest;
        neededScore = (int)Round.access().GetTargetScore(ante, round);

        //Call a function that passes the hand type, and the selected cards, and return only the cards that are part of a valid hand
        //This will be used to calculate the score of the hand
        if (handType == "")
        {
            Debug.LogWarning("No valid hand found. Please select a valid hand.");
            return;
        }
        else if (handType == "HighCard")
        {
            // If the hand type is HighCard, we only need one card
            highCard = currentHandManager.GetHighCard(playedPCards);
            playedPCards.Clear(); // Clear the playedPCards list to avoid duplicates
            playedPCards.Add(highCard); // Add the high card to the playedPCards list
        }
        else
        { 
            playedPCards.Clear(); // Clear the playedPCards list to avoid duplicates

            //Get the list of cards from the current hand manager based on the hand type
            playedPCards = currentHandManager.GetListOfCards(handType); 
        }

        //Start the card scoring process
        StartCoroutine(CardScoring());
    }

    //Resets the different variables and texts once round is finished
    public void EndOfRound()
    {
        Game.access().currentChipAmount = 0;
        roundScoreText.text = "0";
        redScoreText.text = "0";
        blueScoreText.text = "0"; 
        currentChips = 0;
        currentMult = 0;
        totalScore = 0;
        handType = "";
    }


    private IEnumerator CardScoring()
    {
        //Go through cards and add their scores. Wait for a small time
        //before going to the next card
        int numCards = playedPCards.Count;
        int i = 0;

        if (numCards == 1)
        {
            currentChips += playedPCards[0].chips;
            currentMult += playedPCards[0].multiplier;
            Debug.Log("Current Chips: " + currentChips.ToString());
            Debug.Log("Current Mult: " + currentMult.ToString());

            //Update the text UI.
            //Should have a UI element that is shown in realtime
            shakeScreen.StartShake();
            blueScoreText.text = currentChips.ToString();
            redScoreText.text = currentMult.ToString();
        }
        else if (numCards > 0)
        {
            while (i < numCards)
            {
                currentChips += playedPCards[i].chips;
                currentMult += playedPCards[i].multiplier;
                Debug.Log("Current Chips: " + currentChips.ToString());
                Debug.Log("Current Mult: " + currentMult.ToString());

                //Update the text UI.
                //Should have a UI element that is shown in realtime
                shakeScreen.StartShake();
                blueScoreText.text = currentChips.ToString();
                redScoreText.text = currentMult.ToString();

                //Should check for mentors and detect if they need to add any chips/mults/effects

                //Increment and wait to go to next card
                i++;
                yield return new WaitForSecondsRealtime(waitIncrement);
            }
        }

        SetTotal();
        yield return new WaitForSecondsRealtime(1f);

        //Start next round proceedings if the player chip count is greater than or equal to the needed score
        if (neededScore <= player.chipCount)
        {
            EndOfRound();
            TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
            transitionManager.TransitionToEndOfRoundScreen();
        }

        //If player runs out of hands, game over
        else if (Player.access().handCount <= 0)
        {
            EndOfRound();
            TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();

            //Maybe implement a if statement here to check if the player has the amount of chips to continue
            //If not, then transition to defeat screen
            transitionManager.TransitionToDefeatScreen();
        }
    }
    
    //Calculate the totalChips earned from hand, add it to player chipCount
    //and display change in the UI.
    private void SetTotal()
    {
        totalScore = currentChips * currentMult;
        player.chipCount += totalScore;
        roundScoreText.text = player.chipCount.ToString();
    }

    //Takes in hand name and gets chips and mult of hand
    private void SetHandScore(string name)
    {
        switch (name)
        {
            case "HighCard":
                currentChips = player.handTable[TextbookName.HighCard].GetCurrChips();
                currentMult = player.handTable[TextbookName.HighCard].GetCurrMult();
                break;
            case "Pair":
                currentChips = player.handTable[TextbookName.Pair].GetCurrChips();
                currentMult = player.handTable[TextbookName.Pair].GetCurrMult();
                break;
            case "TwoPair":
                currentChips = player.handTable[TextbookName.TwoPair].GetCurrChips();
                currentMult = player.handTable[TextbookName.TwoPair].GetCurrMult();
                break;
            case "ThreeKind":
                currentChips = player.handTable[TextbookName.ThreeKind].GetCurrChips();
                currentMult = player.handTable[TextbookName.ThreeKind].GetCurrMult();
                break;
            case "Straight":
                currentChips = player.handTable[TextbookName.Straight].GetCurrChips();
                currentMult = player.handTable[TextbookName.Straight].GetCurrMult();
                break;
            case "Flush":
                currentChips = player.handTable[TextbookName.Flush].GetCurrChips();
                currentMult = player.handTable[TextbookName.Flush].GetCurrMult();
                break;
            case "FullHouse":
                currentChips = player.handTable[TextbookName.FullHouse].GetCurrChips();
                currentMult = player.handTable[TextbookName.FullHouse].GetCurrMult();
                break;
            case "FourKind":
                currentChips = player.handTable[TextbookName.FourKind].GetCurrChips();
                currentMult = player.handTable[TextbookName.FourKind].GetCurrMult();
                break;
            case "StraightFlush":
                currentChips = player.handTable[TextbookName.StraightFlush].GetCurrChips();
                currentMult = player.handTable[TextbookName.StraightFlush].GetCurrMult();
                break;
            case "FiveKind":
                currentChips = player.handTable[TextbookName.FiveKind].GetCurrChips();
                currentMult = player.handTable[TextbookName.FiveKind].GetCurrMult();
                break;
            case "FlushHouse":
                currentChips = player.handTable[TextbookName.FlushHouse].GetCurrChips();
                currentMult = player.handTable[TextbookName.FlushHouse].GetCurrMult();
                break;
            case "FlushFive":
                currentChips = player.handTable[TextbookName.FlushFive].GetCurrChips();
                currentMult = player.handTable[TextbookName.FlushFive].GetCurrMult();
                break;
            default:
                return;
        }
    }

    //  Return the current chips
    public int GetCurrentChips()
    {
        return currentChips;
    }

    //  Set the current chips (blue score) and update text
    public void SetCurrentChips(int chips)
    {
        currentChips = chips;
        blueScoreText.text = currentChips.ToString();
    }

    //  Returns the current mult
    public int GetCurrentMult()
    {
        return currentMult;
    }

    //  Set the current mult (red score) and update text
    public void SetCurrentMult(int mult)
    {
        currentMult = mult;
        redScoreText.text = currentMult.ToString();
    }

    //  Returns the current hand type of the hand being played as a string
    public string GetCurrentHandType()
    {
        return handType;
    }

    //  Usually called from certain mentors, sequenced after filtering the selectedPCards
    public List<PCard> GetScoredPCards()
    {
        return playedPCards;
    }

    //  Returns this round's needed score
    public BigInteger GetNeededScore()
    {
        return neededScore;
    }

    //  For specific Mentors that trigger PostHand after SetTotal() is called
    public void SetRoundScore(BigInteger bigInteger)
    {
        player.chipCount = bigInteger;
        roundScoreText.text = bigInteger.ToString();
    }
}
