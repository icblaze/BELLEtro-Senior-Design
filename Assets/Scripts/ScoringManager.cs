//Script is used to complete the scoring aspect of the game.
//This includes the chips/mults gained from the cards in addition
//to the mentors.

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;


public class ScoringManager : MonoBehaviour
{
    private DeleteCard deleteCardScript;
    private List<PCard> playedPCards;                                //List of PCards played in the hand   
    private PCard highCard;                                     //High card for the hand  
    private List<GameObject> selectedCards = new List<GameObject>(); //list of selected cards
    private List<PCard> heldHand;
    private CurrentHandManager currentHandManager;                   //Current hand manager to get the current hand
    private ShakeScreen shakeScreen;       //ShakeScreen instance variable
    private SFXManager sfxManager; //SFXManager instance variable
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
    private bool isScoring = false;

    //  Adjust time of scoring manager between each score increment
    private readonly float waitIncrement = 0.5f;

    //  Call to MentorBufferManager
    private MentorBufferManager mentorBuffer = MentorBufferManager.access();
    private CardModifier cardModifier = CardModifier.access();

    private static ScoringManager instance;  //EndOfRoundManager instance varaiable

    //Singleton for the ScoringManager
    public static ScoringManager access()
    {
        return instance;
    }

    // Enforce singleton instance
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Optional: prevent duplicates
            return;
        }

        instance = this;
    }

    public void Start()
    {
        sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
        shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();

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
    public IEnumerator GetScoring()
    {
        //Get PCards and Regular Cards
        playedPCards = deleteCardScript.GetSelectedPCards();
        Debug.Log("Played P Cards Count: " + playedPCards.Count);

        selectedCards = deleteCardScript.GetSelectedCards();

        //Set heldHand list (hand that exclude played PCards)
        Deck.access().SetHeldHand(playedPCards);
        heldHand = Deck.access().heldHand;

        //Get current hand and scores for it
        handType = currentHandManager.findCurrentHand(playedPCards);
        SetHandScore(handType);
    
        int ante = Game.access().GetAnte();
        int round = Game.access().GetRound();
        neededScore = (int)Round.access().GetTargetScore(ante, round);

        //  Check all played cards (even not part of valid hand)
        mentorBuffer.RunBufferImmediate(UseLocation.AllCards);

        //Call a function that passes the hand type, and the selected cards, and return only the cards that are part of a valid hand
        //This will be used to calculate the score of the hand

        //  If player is in possesion of the "Recess" then don't bother trimming the selected cards
        if (!Player.access().mentorDeck.Any(mentor => mentor.name == MentorName.Recess))
        {
            if (handType == "")
            {
                Debug.LogWarning("No valid hand found. Please select a valid hand.");
                yield return null;
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
        }

        //Increment handsPlayed
        int handsPlayed = VictoryManager.access().GetHandsPlayed();
        VictoryManager.access().SetHandsPlayed(handsPlayed + 1);

        //Start the card scoring process
        yield return StartCoroutine(CardScoring());
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

        if (MentorBufferManager.mentorBuffers[UseLocation.AllCards].Count >= 1)
        {
            yield return new WaitForSecondsRealtime(waitIncrement);
        }

        //  Run the initial Mentor buffer
        yield return mentorBuffer.RunBuffer(UseLocation.Initial);

        isScoring = true;

        //  Run the Retrigger mentors/seals
        yield return mentorBuffer.RunRetriggerBuffer(playedPCards);
        foreach (PCard playedCard in playedPCards)
        {
            yield return cardModifier.UseSeal(playedCard, UseLocation.Retrigger);
        }

        //  Playing Each Scored Card in Hand
        foreach (PCard playedCard in playedPCards)
        {
            if (playedCard.isDisabled)
            {
                playedCard.replayCounter = 0;
                continue;
            }

            do
            {
                //  Pre Card
                yield return cardModifier.UseEnhancement(playedCard, UseLocation.PreCard);
                yield return cardModifier.UseSeal(playedCard, UseLocation.PreCard);
                yield return cardModifier.UseEdition(playedCard, UseLocation.PreCard);
                yield return mentorBuffer.RunBuffer(UseLocation.PreCard, playedCard);

                //  Play Card
                currentChips += playedCard.chips;
                currentMult += playedCard.multiplier;
                Debug.Log("Current Chips: " + currentChips.ToString());
                Debug.Log("Current Mult: " + currentMult.ToString());
                sfxManager.CardScoreSFX();

                //Update the text UI.
                //Should have a UI element that is shown in realtime
                shakeScreen.StartShake();
                blueScoreText.text = currentChips.ToString();
                redScoreText.text = currentMult.ToString();
                yield return new WaitForSecondsRealtime(waitIncrement);

                //Post Card (XMult usually)
                yield return cardModifier.UseEnhancement(playedCard, UseLocation.PostCard);
                yield return cardModifier.UseEdition(playedCard, UseLocation.PostCard);
                yield return mentorBuffer.RunBuffer(UseLocation.PostCard, playedCard);

                playedCard.replayCounter--;

            } while (playedCard.replayCounter >= 0);

            //  Reset playedCard replayCounter
            playedCard.replayCounter = 0;
        }

        //  Playing From Draw Cards
        foreach (PCard heldCard in heldHand)
        {
            if(heldCard.isDisabled)
            {
                continue;
            }
            //  PreFromDraw (Mentors)
            yield return mentorBuffer.RunBuffer(UseLocation.PreFromDraw, heldCard);

            //  PostFromDraw (Enhancement)
            yield return cardModifier.UseEnhancement(heldCard, UseLocation.PostFromDraw);
        }

        //  Go through Post Mentor Buffer and consider editions
        yield return mentorBuffer.RunPostBuffer();


        SetTotal();

        isScoring = false;
        yield return new WaitForSecondsRealtime(1f);

        //   Run the PostHand Mentor buffer
        yield return mentorBuffer.RunBuffer(UseLocation.PostHand);

        //Start next round proceedings if the player chip count is greater than or equal to the needed score
        if (neededScore <= player.chipCount)
        {
            //  Activate PostBlind buffer for held cards
            foreach (PCard heldCard in heldHand)
            {
                yield return cardModifier.UseEnhancement(heldCard, UseLocation.PostBlind);
                yield return cardModifier.UseSeal(heldCard, UseLocation.PostBlind);
            }
            yield return mentorBuffer.RunBuffer(UseLocation.PostBlind);

            if (Game.access().GetAnte() == 8 && Game.access().GetRound() == 3)//If game is won, transition to victory screen
            {
                TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
                transitionManager.TransitionToVictoryScreen();
            }
            else//If regular round is won, progress through rounds
            {
                EndOfRound();
                TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();
                transitionManager.TransitionToEndOfRoundScreen();
            }

            
        }

        //If player runs out of hands, game over
        else if (Player.access().handCount <= 0)
        {
            EndOfRound();
            TransitionManager transitionManager = GameObject.FindGameObjectWithTag("TransitionManager").GetComponent<TransitionManager>();

            //  Possible that mentors change the chip value before game over check
            yield return mentorBuffer.RunBuffer(UseLocation.PostBlind);

            //  Check with chips as a last chance
            if (neededScore <= player.chipCount)
            {
                EndOfRound();
                transitionManager.TransitionToEndOfRoundScreen();
            }
            else
            {
                transitionManager.TransitionToDefeatScreen();   //  Transition to Game Over Defeat screen
            }
        }
    }

    //Calculate the totalChips earned from hand, add it to player chipCount
    //and display change in the UI.
    private void SetTotal()
    {
        totalScore = currentChips * currentMult;
        if (totalScore > VictoryManager.access().GetBestHand())
        {
            VictoryManager.access().SetBestHand(totalScore);
        }
        player.chipCount += totalScore;
        roundScoreText.text = player.chipCount.ToString();
    }

    //Takes in hand name and gets chips and mult of hand (also increment playCount
    private void SetHandScore(string name)
    {
        switch (name)
        {
            case "HighCard":
                currentChips = player.handTable[TextbookName.HighCard].GetCurrChips();
                currentMult = player.handTable[TextbookName.HighCard].GetCurrMult();
                player.handTable[TextbookName.HighCard].timesPlayed++;
                break;
            case "Pair":
                currentChips = player.handTable[TextbookName.Pair].GetCurrChips();
                currentMult = player.handTable[TextbookName.Pair].GetCurrMult();
                player.handTable[TextbookName.Pair].timesPlayed++;
                break;
            case "TwoPair":
                currentChips = player.handTable[TextbookName.TwoPair].GetCurrChips();
                currentMult = player.handTable[TextbookName.TwoPair].GetCurrMult();
                player.handTable[TextbookName.TwoPair].timesPlayed++;
                break;
            case "ThreeKind":
                currentChips = player.handTable[TextbookName.ThreeKind].GetCurrChips();
                currentMult = player.handTable[TextbookName.ThreeKind].GetCurrMult();
                player.handTable[TextbookName.ThreeKind].timesPlayed++;
                break;
            case "Straight":
                currentChips = player.handTable[TextbookName.Straight].GetCurrChips();
                currentMult = player.handTable[TextbookName.Straight].GetCurrMult();
                player.handTable[TextbookName.Straight].timesPlayed++;
                break;
            case "Flush":
                currentChips = player.handTable[TextbookName.Flush].GetCurrChips();
                currentMult = player.handTable[TextbookName.Flush].GetCurrMult();
                player.handTable[TextbookName.Flush].timesPlayed++;
                break;
            case "FullHouse":
                currentChips = player.handTable[TextbookName.FullHouse].GetCurrChips();
                currentMult = player.handTable[TextbookName.FullHouse].GetCurrMult();
                player.handTable[TextbookName.FullHouse].timesPlayed++;
                break;
            case "FourKind":
                currentChips = player.handTable[TextbookName.FourKind].GetCurrChips();
                currentMult = player.handTable[TextbookName.FourKind].GetCurrMult();
                player.handTable[TextbookName.FourKind].timesPlayed++;
                break;
            case "StraightFlush":
                currentChips = player.handTable[TextbookName.StraightFlush].GetCurrChips();
                currentMult = player.handTable[TextbookName.StraightFlush].GetCurrMult();
                player.handTable[TextbookName.StraightFlush].timesPlayed++;
                break;
            case "FiveKind":
                currentChips = player.handTable[TextbookName.FiveKind].GetCurrChips();
                currentMult = player.handTable[TextbookName.FiveKind].GetCurrMult();
                player.handTable[TextbookName.FiveKind].timesPlayed++;
                break;
            case "FlushHouse":
                currentChips = player.handTable[TextbookName.FlushHouse].GetCurrChips();
                currentMult = player.handTable[TextbookName.FlushHouse].GetCurrMult();
                player.handTable[TextbookName.FlushHouse].timesPlayed++;
                break;
            case "FlushFive":
                currentChips = player.handTable[TextbookName.FlushFive].GetCurrChips();
                currentMult = player.handTable[TextbookName.FlushFive].GetCurrMult();
                player.handTable[TextbookName.FlushFive].timesPlayed++;
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

    public void IncrementCurrentChips(int chips)
    {
        shakeScreen.StartShake();
        currentChips += chips;
        blueScoreText.text = currentChips.ToString();
        Debug.Log("Current Chips: " + currentChips.ToString());
    }

    //  Returns the current mult
    public int GetCurrentMult()
    {
        return currentMult;
    }

    //  Set the current mult (red score) and update text
    public void SetCurrentMult(int mult)
    {
        shakeScreen.StartShake();
        currentMult = mult;
        sfxManager.MultScoreSFX();
        redScoreText.text = currentMult.ToString();
        Debug.Log("Current Mult: " + currentMult.ToString());
    }

    public void IncrementCurrentMult(int mult)
    {
        shakeScreen.StartShake();
        sfxManager.MultScoreSFX();
        currentMult += mult;
        redScoreText.text = currentMult.ToString();
        Debug.Log("Current Mult: " + currentMult.ToString());
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

    //  Check flag to see if in middle of scoring (useful for differentiating Mentor effects)
    public bool GetScoringStatus()
    {
        return isScoring;
    }
}
