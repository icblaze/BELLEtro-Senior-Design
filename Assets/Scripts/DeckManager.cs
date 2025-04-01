using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab; // Assign in Inspector
    public Transform playingCardGroup; // Assign PlayingCardGroup in Inspector
    public Transform deckPosition; // New reference to the deck's position
    public List<GameObject> deckCards = new List<GameObject>(); // Holds the deck's cards

    void Start()
    {
        // Initialize deck with placeholder cards
        for (int i = 0; i < 20; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, deckPosition);
            newCard.name = "DeckCard_" + i;

            // Rotate the card and position it slightly offset to create a stack effect
            newCard.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
            newCard.transform.localPosition = new Vector3(
                Random.Range(-0.1f, 0.1f),
                i * -0.05f, // Slight vertical offset to create stacking effect
                0
            );

            deckCards.Add(newCard);
        }
    }

    public void DrawCard()
    {
        if (deckCards.Count > 0 && playingCardGroup.childCount < 7) // Limit to 7 cards
        {
            GameObject drawnCard = deckCards[0]; // Take the first card
            deckCards.RemoveAt(0);
            drawnCard.transform.SetParent(playingCardGroup);
            drawnCard.transform.localScale = Vector3.one;
            drawnCard.transform.localPosition = Vector3.zero; // Reset position in hand

            // Optionally, rearrange remaining deck cards after drawing
            RearrangeDeckCards();

            Debug.Log($"Drawn new card: {drawnCard.name}");
        }
        else
        {
            Debug.LogWarning("Deck is empty or playing hand is full! No more cards to draw.");
        }
    }

    private void RearrangeDeckCards()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            deckCards[i].transform.localPosition = new Vector3(
                Random.Range(-0.1f, 0.1f),
                i * -0.05f,
                0
            );
            deckCards[i].transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
        }
    }
}