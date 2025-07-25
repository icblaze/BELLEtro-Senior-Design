// This class handles the logic for moving the selected cards from the players hand
// to the play area where the hands are usually scored. This class also handles deleting
// and retrieving new cards from the deck to the players hand so the player can continue playing.
//Current Devs:
//Zacharia Alaoui(Zacharia Alaoui)
//Andy Van(Andy Van)

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class PlayHand : MonoBehaviour
{
    public Button playHandButton; // Assign in Inspector
    public RectTransform playArea; // Assign a UI GameObject at the center
    public Text cardCountText; // Assign a UI text to display the count
    private List<GameObject> selectedCards = new List<GameObject>(); // Cards that are selected
    private DeleteCard deleteCardScript;                             // Reference to DeleteCard
    private DeckManager deckManager;                                 // Reference to DeckManager
    public TextMeshProUGUI handsLeft;
    private ScoringManager ScoringManager;
    private GameObject[] roundCovers; //  Covers for the hand and mentors

    private static int currHandCount;
    private Transform playingCardGroup;

    public bool cashPenalty = false;
    public bool drawThree = false;
    public bool handful = false;

    // Look into incorporating a save system
    void Awake()
    {
        ResetRoundValues();
    }

    void Start()
    {
        playHandButton.onClick.AddListener(PlaySelectedCards);
        deleteCardScript = FindFirstObjectByType<DeleteCard>(); // Find DeleteCard in the scene
        deckManager = FindFirstObjectByType<DeckManager>();     // Find DeckManager in the scene
        handsLeft = GameObject.Find("Hands Number Text").GetComponent<TextMeshProUGUI>();
        Player player = Player.access();
    
        if (playingCardGroup == null)
        {
            var go = GameObject.Find("PlayingCardGroup");
            if (go != null)
                playingCardGroup = go.transform;
            else
                Debug.LogError("DeleteCard: playingCardGroup not assigned AND no GameObject named 'PlayingCardGroup' found!");
        }

        Debug.Log($"{player.handCount}");
        if (deleteCardScript == null)
            Debug.LogError("PlayHand: DeleteCard script not found in scene!");

        if (deckManager == null)
            Debug.LogError("PlayHand: DeckManager script not found in scene!");

        //  Find cover objects that will block elements of the round while scoring
        roundCovers = GameObject.FindGameObjectsWithTag("BlockWhileScoring");

        if (roundCovers != null)
        {
            //  There should be 2, but just in case
            foreach (GameObject cover in roundCovers)
            {
                cover.SetActive(false);
            }
        }
    }

    //  Sets discards to be the maxDiscards after blind select or end of round
    public void ResetHandCount()
    {
        Player.access().handCount = Player.access().maxHandCount;
        currHandCount = Player.access().handCount;      //Decrease Hand count of the player
        handsLeft.text = currHandCount.ToString("0");
    }

    //  Sets hand after reset is called (by Mentors)
    public void SetHandCount(int hands)
    {
        Player.access().handCount = hands;
        currHandCount = Player.access().handCount;      //Decrease Hand count of the player
        handsLeft.text = currHandCount.ToString("0");
    }

    //This function initiates moving the selected cards to the play area 
    //after the player clicks on the play hand button
    public void PlaySelectedCards()
    {
        selectedCards = deleteCardScript.GetSelectedCards(); // Get selected cards

        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected to play!");
            return;
        }

        if (selectedCards.Count > 5)
        {
            Debug.LogWarning("Cannot play more than 5 cards!");
            return;
        }
        if (handful == true && selectedCards.Count != 5)//If handful active and not playing 5 cards
        {
            Debug.LogWarning("Must Play 5 cards!");
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            SFXManager sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
            sfxManager.NoSFX();
            return;
        }
        currHandCount = Player.access().handCount;
        if (currHandCount == 0)
        {
            Debug.LogWarning("Cannot play cards since you have reached zero hands!");
            return;
        }

        //  Trying to play a card that isn't part of your hand
        foreach (GameObject card in selectedCards)
        {
            if (!card.transform.IsChildOf(playingCardGroup))
            {
                Debug.LogWarning($"Cannot delete {card?.name}; not a child of playingCardGroup.");
                return;
            }
        }

        Debug.Log($"{gameObject.name} called PlaySelectedCards()");

        currHandCount = Round.access().DecreaseHandCount();      //Decrease Hand count of the player
        handsLeft.text = currHandCount.ToString("0");

        if (cashPenalty)
        {
            Player.access().moneyCount--;
            ShopManager.access().UpdateMoneyDisplay();
        }

        //Call a function here that clears all the Lists from currentHandManager

        // Move selected cards to the play area
        StartCoroutine(MoveCardsToPlayArea());
    }

    private IEnumerator MoveCardsToPlayArea()
    {
        if(roundCovers != null)
        {
            //  There should be 2, but just in case
            foreach(GameObject cover in roundCovers)
            {
                cover.SetActive(true);
            }
        }


        Debug.Log("MoveCardsToPlayArea started");

        float offsetX = -((selectedCards.Count - 1) * 50f); // Adjusts card positioning

        for (int i = 0; i < selectedCards.Count; i++)
        {
            GameObject card = selectedCards[i];
            
            if (card == null)
            {
                Debug.LogError($"Card at index {i} is NULL!");
                continue;
            }

            RectTransform cardRect = card.GetComponent<RectTransform>();

            if (cardRect == null)
            {
                Debug.LogError($"Card at index {i} does NOT have a RectTransform!");
                continue;
            }

            // ✅ Move to play area
            card.transform.SetParent(playArea, false);

            card.GetComponent<Card>().isPlayed = true;

            Vector2 targetPosition = new Vector2(offsetX + (i * 150f), 0);
            StartCoroutine(MoveCard(card, targetPosition));
        }

        //Call a function here to Calculate the score of the hand that was played.
        //selectedCards is a list of cards that were selected by the player and these
        //cards have been moved to the PlayArea.
        ScoringManager scoringManager = FindFirstObjectByType<ScoringManager>();
        if (scoringManager == null)
        {
            Debug.LogError("PlayHand: ScoringManager script not found in scene!");
            yield break; // Exit if ScoringManager is not found
        }

        yield return scoringManager.GetScoring();

        // ✅ Remove cards and draw new ones
        int cardsPlayed = selectedCards.Count;

        foreach (GameObject card in selectedCards)
        {
            if (card != null)
            {
                deleteCardScript.RemoveSelectedCard(card);
                Destroy(card);
            }
        }

        selectedCards.Clear();  //  might be redundant
        
        PCard[] newCards;
        if (drawThree == true)//If special blind is active, draw three
        {
            newCards = Deck.access().drawCards(3);
        }
        else
        {
            //  Draw from deck equal to amount played
            newCards = Deck.access().drawCards(cardsPlayed);
        }

        //  Break glass cards that were marked after put into drawnCards pile
        Deck.access().DestroyGlassCards();

        StartCoroutine(RefillNextFrame(newCards));
    }

    private IEnumerator RefillNextFrame(PCard[] newCards)
    {
        // wait one frame so the Slots are truly empty
        yield return null;

        //  Clean up extra slots if effects that given it have been sold/used up
        int extraSlots = deckManager.GetSlotCount() - Player.access().handSize;

        while (extraSlots > 0)
        {
            Transform emptySlot = deckManager.GetFirstEmptySlot();
            if (emptySlot != null)
            {
                Destroy(emptySlot.gameObject);
            }
            extraSlots--;
            yield return null;
        }

        // here’s the only change: use DrawCard() so it honors your Slot layout
        for (int i = 0; i < newCards.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            deckManager.DrawCard(newCards[i]);
        }

        int missingSlots;

        if (drawThree == true)//If special blind is active, ignore handSize
        {
            missingSlots = 0;
        }
        else
        {
            missingSlots = Player.access().handSize - deckManager.GetSlotCount();
        }

        if (missingSlots > 0)
        {
            PCard[] extraCards = Deck.access().drawCards(missingSlots);

            foreach (PCard extra in extraCards)
            {
                deckManager.DrawNewSlot(extra);
            }
        }

        if (roundCovers != null)
        {
            //  There should be 2, but just in case
            foreach (GameObject cover in roundCovers)
            {
                cover.SetActive(false);
            }
        }
    }

    //This function moves the card to the target position with a smooth animation
    private IEnumerator MoveCard(GameObject card, Vector2 targetPosition)
    {
        float duration = 0.5f; // Move animation duration
        float elapsed = 0f;
        RectTransform cardRect = card.GetComponent<RectTransform>();

        if (cardRect == null)
        {
            Debug.LogError("MoveCard Error: Card does not have a RectTransform!");
            yield break;
        }

        Vector2 startPos = cardRect.anchoredPosition;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cardRect.anchoredPosition = Vector2.Lerp(startPos, targetPosition, elapsed / duration);
            yield return null;
        }

        cardRect.anchoredPosition = targetPosition; // Ensure exact final position
    }

    //This function resets the necessary variables for the player when a new round starts
    public void ResetRoundValues()
    {
        Player.access().handCount = 4;
        Player.access().discards = 4;
        Player.access().moneyCount = 0;
        Player.access().chipCount = 0;
        Player.access().discount = 1.0f; // Reset discount to 100%
        Player.access().InitializeHandTable();
    }
}
