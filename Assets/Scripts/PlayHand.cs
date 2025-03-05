using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayHand : MonoBehaviour
{
    public Button playHandButton; // Assign in Inspector
    public RectTransform playArea; // Assign a UI GameObject at the center
    public Text cardCountText; // Assign a UI text to display the count
    public float displayDuration = 2.0f; // Time before cards disappear

    private List<GameObject> selectedCards = new List<GameObject>(); // Cards that are selected
    private DeleteCard deleteCardScript; // Reference to DeleteCard

    void Start()
    {
        playHandButton.onClick.AddListener(PlaySelectedCards);
        deleteCardScript = FindFirstObjectByType<DeleteCard>(); // Find DeleteCard in the scene
    }

    void PlaySelectedCards()
    {
        selectedCards = deleteCardScript.GetSelectedCards(); // Get selected cards

        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected to play!");
            return;
        }

        // Move selected cards to the play area
        StartCoroutine(MoveCardsToCenter());
    }

    IEnumerator MoveCardsToCenter()
    {
        Debug.Log("MoveCardsToCenter started");

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

            // ✅ Set PlayArea as the new parent
            card.transform.SetParent(playArea, false);

            // ✅ Calculate the correct position relative to PlayArea
            Vector2 targetPosition = new Vector2(offsetX + (i * 100f), 0);
            StartCoroutine(MoveCard(card, targetPosition));

        }

        yield return new WaitForSeconds(displayDuration);

        foreach (GameObject card in selectedCards)
        {
            if (card != null)
            {
                deleteCardScript.RemoveSelectedCard(card);
                Destroy(card);
            }
        }

        selectedCards.Clear();
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


}
