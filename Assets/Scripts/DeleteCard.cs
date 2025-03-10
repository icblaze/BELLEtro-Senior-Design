using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DeleteCard : MonoBehaviour
{
    public Button deleteButton; // Assign in Inspector
    public List<GameObject> playerHand = new List<GameObject>(); // Player's hand of cards
    private List<GameObject> selectedCards = new List<GameObject>(); // Store selected cards

    private GameObject playingCardGroup;
    private GameObject jokerCardGroup;

    void Start()
    {
        // Find groups dynamically to avoid errors if names change
        playingCardGroup = GameObject.Find("PlayingCardGroup");
        jokerCardGroup = GameObject.Find("JokerCardGroup");

        if (deleteButton != null)
        {
            deleteButton.onClick.AddListener(RemoveSelectedCards);
        }
        else
        {
            Debug.LogError("DeleteCard: Delete button is not assigned!");
        }
    }

    public void AddSelectedCard(GameObject card)
    {
        if (card == null)
        {
            Debug.LogError("AddSelectedCard: Trying to add a NULL card!");
            return;
        }

        if (!selectedCards.Contains(card))
        {
            selectedCards.Add(card);
            Debug.Log($"Card {card.name} added to selection.");
        }
    }

    public void RemoveSelectedCard(GameObject card)
    {
        if (card == null)
        {
            Debug.LogError("RemoveSelectedCard: Trying to remove a NULL card!");
            return;
        }

        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            Debug.Log($"Card {card.name} removed from selection.");
        }
    }

    void RemoveSelectedCards()
    {
        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected for deletion!");
            return;
        }

        List<GameObject> cardsToRemove = new List<GameObject>();

        foreach (GameObject card in selectedCards)
        {
            string cardParentName = card.transform.parent?.name;
            Debug.Log($"Checking deletion for {card.name}, Parent: {cardParentName}");

            // Ensure card is inside PlayingCardGroup and its name starts with "Card "
            if (IsInsideGroup(card, playingCardGroup) && card.name.StartsWith("Card "))
            {
                cardsToRemove.Add(card);
            }
            else
            {
                Debug.LogWarning($"Cannot delete {card.name} because it is not inside PlayingCardGroup!");
            }
        }

        // Delete allowed cards
        foreach (GameObject card in cardsToRemove)
        {
            if (playerHand.Contains(card))
            {
                playerHand.Remove(card);
            }

            if (card != null)
            {
                Debug.Log($"Deleting card: {card.name}");
                Destroy(card);
            }
        }

        selectedCards.Clear();
    }

    private bool IsInsideGroup(GameObject card, GameObject group)
    {
        if (group == null) return false;

        Transform parent = card.transform.parent;
        while (parent != null)
        {
            if (parent == group.transform)
            {
                return true;
            }
            parent = parent.parent; // Traverse up the hierarchy
        }
        return false;
    }

    public List<GameObject> GetSelectedCards()
    {
        return new List<GameObject>(selectedCards);
    }
}
