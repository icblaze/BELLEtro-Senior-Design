using System.Collections;
using System.Collections.Generic;
using System.Linq;            // ← for Linq helpers
using Unity.VisualScripting;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private int deckIndex;          
    [Header("Prefabs & Transforms")]
    public GameObject cardPrefab;                                           // Assign your card prefab asset
    public GameObject slotPrefab;
    public Transform playingCardGroup;                                      // Assign PlayingCardGroup in Inspector
    public Transform deckPosition;                                          // Where the deck sits
    public RectTransform pinkCardImage;

    public HorizontalCardHolder horizontalCardHolder;

     void Awake()
    {
        // This will run before any Start() method and clear the old "ghost" reference.
        Card.ResetStaticPanel();
    }

    void Start()
    {
        //Deck deck = Deck.access();

        // //In the future we might change this so we can modify the hand size based off the selected deck.
        // setMaxHandCount(8);
        // maxCardsInHand = getMaxHandCount();

        // //Debug.LogError($"Deck Counter Updated: {Deck.counter}");

        // if (deck.deckCards.Count < 56)
        // {
        //     Debug.LogError("Not enough cards in deckCardsData! Check deck initialization.");
        //     return;
        // }


        // // Fill the deck with maxCardsInHand placeholder cards
        // for (int i = 0; i < maxCardsInHand; i++)
        // {
        //     //Don't spawn all 56 cards, have one face down card in the deck
        //     GameObject newCard = Instantiate(cardPrefab, deckPosition);
        //     CardObject cardComponent = newCard.AddComponent<CardObject>(); //Attach the CardObject script to the GameObject for each card.

        //     newCard.name = $"DeckCard_{i}";
        //     newCard.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
        //     newCard.transform.localPosition = new Vector3(
        //         Random.Range(-0.1f, 0.1f),
        //         i * -0.05f,
        //         0f
        //     );

        //     //Assign a random card from the deck and assign it to the cardComponent
        //     cardComponent.cardData = deck.deckCards[i];

        //     deckCards.Add(newCard);
        // }
        pinkCardImage.SetAsLastSibling();

    }

    // Draws given card into the first empty "Slot" under playingCardGroup.
    public void DrawCard(PCard pcard)
    {
        if(horizontalCardHolder == null)
        {
            horizontalCardHolder = playingCardGroup.GetComponentInParent<HorizontalCardHolder>();
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

        // instantiate it at deck position
        GameObject drawn = Instantiate(cardPrefab, deckPosition);

        //  Get Card component and assign PCard
        Card cardComponent = drawn.GetComponent<Card>();
        cardComponent.AssignPCard(pcard);
        cardComponent.PointerEnterEvent.AddListener(horizontalCardHolder.CardPointerEnter);
        cardComponent.PointerExitEvent.AddListener(horizontalCardHolder.CardPointerExit);
        cardComponent.BeginDragEvent.AddListener(horizontalCardHolder.BeginDrag);
        cardComponent.EndDragEvent.AddListener(horizontalCardHolder.EndDrag);

        //  Reparent into empty slot
        drawn.name = cardPrefab.name; // strip the "(Clone)" if you like
        drawn.transform.SetParent(emptySlot, false);
        drawn.transform.localScale = Vector3.one;

        Debug.Log($"Drew {drawn.name} into slot {emptySlot.name}");

        //  Set player's hand to match new cards
        horizontalCardHolder.RefreshVisual();
    }

    //  When more slots are needed, draw card into new slot
    public void DrawNewSlot(PCard pcard)
    {
        if (horizontalCardHolder == null)
        {
            horizontalCardHolder = playingCardGroup.GetComponentInParent<HorizontalCardHolder>();
        }

        GameObject newSlot = Instantiate(slotPrefab, horizontalCardHolder.transform);

        //  Get Card component and assign PCard
        Card cardComponent = newSlot.GetComponentInChildren<Card>();
        cardComponent.AssignPCard(pcard);
        cardComponent.PointerEnterEvent.AddListener(horizontalCardHolder.CardPointerEnter);
        cardComponent.PointerExitEvent.AddListener(horizontalCardHolder.CardPointerExit);
        cardComponent.BeginDragEvent.AddListener(horizontalCardHolder.BeginDrag);
        cardComponent.EndDragEvent.AddListener(horizontalCardHolder.EndDrag);

        //  Set player's hand to match new cards
        horizontalCardHolder.RefreshVisual();
    }

    public int GetSlotCount()
    {
        int slotTransforms = playingCardGroup
            .GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag("Slot"))
            .ToList().Count;

        return slotTransforms;
    }

    public Transform GetFirstEmptySlot()
    {
        List<Transform> slotTransforms = playingCardGroup
            .GetComponentsInChildren<Transform>(true)
            .Where(t => t.CompareTag("Slot"))
            .ToList();

        Transform emptySlot = slotTransforms.FirstOrDefault(s => s.childCount == 0);

        return emptySlot;
    }


    /// <summary>
    /// Draws one card directly into the playingCardGroup—ignores slots and hand-size limits.
    /// </summary>
    //public void DrawCardNoLimit()
    //{
    //    if (deckCards.Count == 0)
    //    {
    //        Debug.LogWarning("Deck is empty!");
    //        return;
    //    }

    //    Game game = Game.access();
    //    deckIndex = game.randomizer(0, deckCards.Count);
    //    GameObject prefab = deckCards[deckIndex];
    //    deckCards.RemoveAt(deckIndex);

    //    // parent it straight under playingCardGroup
    //    GameObject c = Instantiate(prefab, playingCardGroup, false);
    //    c.name = prefab.name;
    //    c.transform.localScale = Vector3.one;

    //    Debug.Log($"Drew {c.name} (no-limit) into hand");
    //}

    /// <summary>
    /// (Optional) If you ever want to reshuffle/re-layout the remaining deck.
    /// </summary>
    //private void RearrangeDeckCards()
    //{
    //    for (int i = 0; i < deckCards.Count; i++)
    //    {
    //        var t = deckCards[i].transform;
    //        t.localPosition = new Vector3(
    //            Random.Range(-0.1f, 0.1f),
    //            i * -0.05f,
    //            0f
    //        );
    //        t.localRotation = Quaternion.Euler(0, 0, Random.Range(-5f, 5f));
    //    }
    //}

}
