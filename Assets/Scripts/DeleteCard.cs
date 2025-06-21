// This class handles the functionality of the delete button, and it handles removing the cards
// from the players hand. This class also retrieves cards from the deck into the empty slots that
// are in the players hand.
// Van Phan(trieu1852000): Implemented the DeleteCard class
// Zacharia Alaoui(ZachariaAlaoui): Added comments throughout the class for better readibility

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeleteCard : MonoBehaviour
{
    [SerializeField] private Button deleteButton;
    [SerializeField] private Transform playingCardGroup;

    private DeckManager deckManager;
    private List<GameObject> selectedCards = new List<GameObject>();    //List of the selected cards the player has selected.

    void Start()
    {
        deckManager = FindObjectOfType<DeckManager>();
        if (deckManager == null)
            Debug.LogError("DeleteCard: no DeckManager in scene!");

        if (playingCardGroup == null)
        {
            var go = GameObject.Find("PlayingCardGroup");
            if (go != null)
                playingCardGroup = go.transform;
            else
                Debug.LogError("DeleteCard: playingCardGroup not assigned AND no GameObject named 'PlayingCardGroup' found!");
        }

        if (deleteButton != null)
            deleteButton.onClick.AddListener(RemoveSelectedCards);
        else
            Debug.LogError("DeleteCard: deleteButton not assigned in Inspector!");
    }

    //Adds the card to the selectedCards list
    public void AddSelectedCard(GameObject card)
    {
        if (card == null) return;
        if (!selectedCards.Contains(card))
            selectedCards.Add(card);
    }

    //This removes the card the player unselected from the selectedCards list
    public void RemoveSelectedCard(GameObject card)
    {
        if (card == null) return;
        if (selectedCards.Contains(card))
            selectedCards.Remove(card);
    }
    
    //This returns the selected cards that the player currently has selected
    public List<GameObject> GetSelectedCards()
    {
        return new List<GameObject>(selectedCards);
    }

    //This function removes the cards that the player played, and clears the selectedCards list.
    //This function also starts a coroutine to fill the player hand with new cards
    void RemoveSelectedCards()
    {
        if (selectedCards.Count > 5)
        {
            Debug.LogWarning("You cannot discard more than 5 cards at a time!");
            // Stop the function here so no cards are deleted.
            return;
        }
        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected for deletion!");
            return;
        }

        var cardsToRemove = new List<GameObject>();
        foreach (var card in selectedCards)
        {
            if (card != null && playingCardGroup != null && card.transform.IsChildOf(playingCardGroup))
                cardsToRemove.Add(card);
            else
                Debug.LogWarning($"Cannot delete {card?.name}; not a child of playingCardGroup.");
        }

        int removedCount = cardsToRemove.Count;
        foreach (var card in cardsToRemove)
        {
            Debug.Log($"Deleting card: {card.name}");
            Destroy(card);
        }

        selectedCards.Clear();
        StartCoroutine(RefillNextFrame(removedCount));
    }

    private IEnumerator RefillNextFrame(int count)
    {
        // wait one frame so the Slots are truly empty
        yield return null;

        // here’s the only change: use DrawCard() so it honors your Slot layout
        for (int i = 0; i < count; i++)
            deckManager.DrawCard();
    }
}
