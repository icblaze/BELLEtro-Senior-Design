using System.Collections.Generic;
using System.Linq;            // ← for Linq helpers
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("Prefabs & Transforms")]
    public GameObject cardPrefab;                                           // Assign your card prefab asset
    public Transform playingCardGroup;                                      // Assign PlayingCardGroup in Inspector
    public Transform deckPosition;                                          // Where the deck sits
    public RectTransform pinkCardImage;
    [Header("Deck Data")]
    public List<GameObject> deckCards = new List<GameObject>();             //List of GameObject cards

    [Header("Settings")]
    private int maxCardsInHand;                                             //This variable will store the maximum cards in a hand for the round

    void Start()
    {
        Deck deck = Deck.access();

        //In the future we might change this so we can modify the hand size based off the selected deck.
        setMaxHandCount(8);
        maxCardsInHand = getMaxHandCount();

        //Debug.LogError($"Deck Counter Updated: {Deck.counter}");

        if (deck.deckCards.Count < 56)
        {
            Debug.LogError("Not enough cards in deckCardsData! Check deck initialization.");
            return;
        }


        // Fill the deck with maxCardsInHand placeholder cards
        for (int i = 0; i < maxCardsInHand; i++)
        {
            //Don't spawn all 56 cards, have one face down card in the deck
            GameObject newCard = Instantiate(cardPrefab, deckPosition);
            CardObject cardComponent = newCard.AddComponent<CardObject>(); //Attach the CardObject script to the GameObject for each card.

            newCard.name = $"DeckCard_{i}";
            newCard.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
            newCard.transform.localPosition = new Vector3(
                Random.Range(-0.1f, 0.1f),
                i * -0.05f,
                0f
            );

            //I need to assign a random card from the deck and assign it to the cardComponent I made
            cardComponent.cardData = deck.deckCards[i];

            deckCards.Add(newCard);
        }
        pinkCardImage.SetAsLastSibling();

        
    }

    /// <summary>
    /// Draws one card into the first empty "Slot" under playingCardGroup.
    /// Honors your maxCardsInHand if you want to gate it.
    /// </summary>
    public void DrawCard()
    {
        if (deckCards.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return;
        }

        // find all children tagged "Slot"
        var slotTransforms = playingCardGroup
            .GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag("Slot"))
            .ToList();

        // pick the first empty one
        Transform emptySlot = slotTransforms.FirstOrDefault(s => s.childCount == 0);
        if (emptySlot == null)
        {
            Debug.LogWarning("No empty slot available!");
            return;
        }

        // remove top card from deck list
        GameObject prefab = deckCards[0];
        deckCards.RemoveAt(0);

        // instantiate it at deck position, then reparent into the slot
        GameObject drawn = Instantiate(prefab, deckPosition);
        drawn.name = prefab.name; // strip the "(Clone)" if you like
        drawn.transform.SetParent(emptySlot, false);
        drawn.transform.localScale = Vector3.one;

        Debug.Log($"Drew {drawn.name} into slot {emptySlot.name}");
    }

    /// <summary>
    /// Draws one card directly into the playingCardGroup—ignores slots and hand-size limits.
    /// </summary>
    public void DrawCardNoLimit()
    {
        if (deckCards.Count == 0)
        {
            Debug.LogWarning("Deck is empty!");
            return;
        }

        GameObject prefab = deckCards[0];
        deckCards.RemoveAt(0);

        // parent it straight under playingCardGroup
        GameObject c = Instantiate(prefab, playingCardGroup, false);
        c.name = prefab.name;
        c.transform.localScale = Vector3.one;

        Debug.Log($"Drew {c.name} (no-limit) into hand");
    }

    /// <summary>
    /// (Optional) If you ever want to reshuffle/re-layout the remaining deck.
    /// </summary>
    private void RearrangeDeckCards()
    {
        for (int i = 0; i < deckCards.Count; i++)
        {
            var t = deckCards[i].transform;
            t.localPosition = new Vector3(
                Random.Range(-0.1f, 0.1f),
                i * -0.05f,
                0f
            );
            t.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
        }
    }

    //Set the maximum hand count for the round.
    public void setMaxHandCount(int handCount)
    {
        maxCardsInHand = handCount;
    }

    //Getter to retrieve the max hand count for the round.
    public int getMaxHandCount()
    {
        return maxCardsInHand;
    }
}
