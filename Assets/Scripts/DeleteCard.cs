using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DeleteCard : MonoBehaviour
{
    [SerializeField] private Button deleteButton;
    [SerializeField] private Transform playingCardGroup;

    private DeckManager deckManager;
    private List<GameObject> selectedCards = new List<GameObject>();

    void Start()
    {
        // find DeckManager
        deckManager = FindObjectOfType<DeckManager>();
        if (deckManager == null)
            Debug.LogError("DeleteCard: no DeckManager in scene!");

        // auto-assign playingCardGroup if left null in Inspector
        if (playingCardGroup == null)
        {
            var go = GameObject.Find("PlayingCardGroup");
            if (go != null)
            {
                playingCardGroup = go.transform;
                Debug.Log("DeleteCard: auto-found PlayingCardGroup at runtime.");
            }
            else
            {
                Debug.LogError("DeleteCard: playingCardGroup not assigned AND no GameObject named 'PlayingCardGroup' found!");
            }
        }

        // hook up the Delete button
        if (deleteButton != null)
            deleteButton.onClick.AddListener(RemoveSelectedCards);
        else
            Debug.LogError("DeleteCard: deleteButton not assigned in Inspector!");
    }

    public void AddSelectedCard(GameObject card)
    {
        if (card == null) return;
        if (!selectedCards.Contains(card))
            selectedCards.Add(card);
    }

    public void RemoveSelectedCard(GameObject card)
    {
        if (card == null) return;
        if (selectedCards.Contains(card))
            selectedCards.Remove(card);
    }

    // so other scripts (PlayHand.cs) can inspect your selection
    public List<GameObject> GetSelectedCards()
    {
        return new List<GameObject>(selectedCards);
    }

    void RemoveSelectedCards()
    {
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
        yield return null;   // wait one frame for Destroy() to complete
        for (int i = 0; i < count; i++)
            deckManager.DrawCardNoLimit();
    }
}
