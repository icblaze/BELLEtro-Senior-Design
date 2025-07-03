using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//Script is used to complete the scoring aspect of the game.
//This includes the chips/mults gained from the cards in addition
//to the mentors.

public class NewMonoBehaviourScript : MonoBehaviour
{
    private DeleteCard deleteCardScript;
    private List<PCard> playedPCards;
    private List<GameObject> selectedCards = new List<GameObject>();
    private CurrentHandManager currentHandManager;
    ShakeScreen shakeScreen = ShakeScreen.access();
    Player player = Player.access();
    private int currentChips;
    private int currentMult;
    private string handType;
    private GameObject roundScore;
    private GameObject blueScore;
    private GameObject redScore;
    private TMP_Text blueScoreText;
    private TMP_Text redScoreText;
    private TMP_Text roundScoreText;
    private int totalScore;

    public void Start()
    {
        //Get delete card script and do null check
        deleteCardScript = FindFirstObjectByType<DeleteCard>();

        if (deleteCardScript == null)
            Debug.LogError("PlayHand: DeleteCard script not found in scene!");

        //Get current hand Manager
        currentHandManager = FindFirstObjectByType<CurrentHandManager>();
        if (currentHandManager == null)
            Debug.LogError("PlayHand: CurrentHandManager script not found in scene!");

        //Get score UI components
        roundScore = GameObject.Find("Round Score");
        roundScoreText = roundScore.GetComponent<TMP_Text>();

        blueScore = GameObject.Find("Blue Score Text");
        blueScoreText = blueScore.GetComponent<TMP_Text>();

        redScore = GameObject.Find("Red Score Text");
        redScoreText = redScore.GetComponent<TMP_Text>();
    }
    
    //Public function to start the scoring process
    public void GetScoring()
    {
        //Get P Cards and Regular Crads
        playedPCards = deleteCardScript.GetSelectedPCards();
        selectedCards = deleteCardScript.GetSelectedCards();

        //Get current hand and scores for it
        handType = currentHandManager.findCurrentHand(playedPCards);
        SetHandScore(handType);

        StartCoroutine(CardScoring());
    }

    private IEnumerator CardScoring()
    {
        //Go through cards and add their scores. Wait for a small time
        //before going to the next card
        int numCards = playedPCards.Count;
        int i = 0;
        while (i < numCards)
        {
            currentChips += playedPCards[i].chips;
            currentMult += playedPCards[i].multiplier;

            //Update the text UI.
            //Should have a UI element that is shown in realtime
            shakeScreen.StartShake();
            blueScoreText.text = currentChips.ToString();
            redScoreText.text = currentMult.ToString();

            //Should check for mentors and detect if they need to add any chips/mults/effects

            //Increment and wait to go to next card
            i++;
            yield return new WaitForSecondsRealtime(.25f);
        }
        SetTotal();
        yield return new WaitForSecondsRealtime(.5f);

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
}
