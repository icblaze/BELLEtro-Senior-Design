// This class handles the logic for moving the selected cards from the players hand
// to the play area where the hands are usually scored. This class also handles deleting
// and retrieving new cards from the deck to the players hand so the player can continue playing.

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
    public float displayDuration = 2.0f; // Time before cards disappear
    private List<GameObject> selectedCards = new List<GameObject>(); // Cards that are selected
    private DeleteCard deleteCardScript;                             // Reference to DeleteCard
    private DeckManager deckManager;                                 // Reference to DeckManager
    public TextMeshProUGUI handsLeft;

    private static int currHandCount;
    private Transform playingCardGroup;

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
    }

    //This function initiates moving the selected cards to the play area 
    //after the player clicks on the play hand button
    void PlaySelectedCards()
    {
        selectedCards = deleteCardScript.GetSelectedCards(); // Get selected cards

        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected to play!");
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

        // Move selected cards to the play area
        StartCoroutine(MoveCardsToPlayArea());
    }

    IEnumerator MoveCardsToPlayArea()
    {
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

            Vector2 targetPosition = new Vector2(offsetX + (i * 100f), 0);
            StartCoroutine(MoveCard(card, targetPosition));
        }

        //Call a function here to Calculate the score of the hand that was played.
        //selectedCards is a list of cards that were selected by the player and these
        //cards have been moved to the PlayArea.

        yield return new WaitForSeconds(displayDuration);

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

        selectedCards.Clear();

        // ✅ Only draw new cards equal to the number of played cards
        for (int i = 0; i < cardsPlayed; i++)
        {
            deckManager.DrawCard();
        }
    }


    IEnumerator MoveCard(GameObject card, Vector2 targetPosition)
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
    }
}
