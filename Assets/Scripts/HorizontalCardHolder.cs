// This Document contains the code for the playing card holder
// This class draws cards at start of blind
// Current Devs:
// Van: setup initial code
// Andy: connected it to draw from Deck class

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class HorizontalCardHolder : MonoBehaviour
{
    [SerializeField] public Card selectedCard;
    [SerializeReference] public Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Spawn Settings")]
    [SerializeField] private int handSize = 8;
    public List<Card> cards;

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    [Header("Special Blind Settings")]
    public bool hideSuitFlag = false;
    public bool disabledSuitFlag = false;
    public SuitName disabledSuitName;

    private Deck deck = Deck.access();
    private DeleteCard deleteScript;

    void Awake()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();
    }

    void Start()
    {
        //StartCoroutine(OnBlindStart());
    }

    public IEnumerator OnBlindStart()
    {
        //  Destroy Existing slots (if any), then wait
        DestroyExistingSlots();
        yield return null;
        yield return null;

        //  Initial draw from deck to hand
        DrawHand(Player.access().handSize);

        //  Draws the cards to the slots visually and assign hand
        RefreshVisual();
    }

    //  Beginning of blind, draw cards from deck into hand
    private void DrawHand(int handSize)
    {
        for (int i = 0; i < handSize; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform);
            newSlot.name = $"Card {i + 1}"; // Assign meaningful names
        }

        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>().ToList();

        //  Draw PCards from the deck for each Card intially
        //  Make sure drawn cards are moved back to deckCards by calling combinePiles
        deck.combinePiles();
        PCard[] pcardArray = deck.drawCards(handSize);
        Debug.Log("This is Deck: " + deck);
        Debug.Log("This is hand size: " + handSize);
        Debug.Log("This is Deck Draw card: " + deck.drawCards(handSize));
        Debug.Log("This is PCard: " + pcardArray);

        int cardCount = 0;
        foreach (Card card in cards)
        {
            card.PointerEnterEvent.AddListener(CardPointerEnter);
            card.PointerExitEvent.AddListener(CardPointerExit);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);
            card.name = $"Card {cardCount + 1}"; // Assign names sequentially
            card.AssignPCard(pcardArray[cardCount]); //  Assign PCard object to each card
            cardCount++;
        }
    }

    public void BeginDrag(Card card)
    {
        //cards = GetComponentsInChildren<Card>().ToList();   //  Refresh card list
        selectedCard = card;
    }

    public void EndDrag(Card card)
    {
        cards = GetComponentsInChildren<Card>().ToList(); //  Refresh card list
        deck.playerHand = cards.Select(card => card.pcard).ToList();

        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0, selectedCard.selectionOffset, 0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;
    }

    public void CardPointerEnter(Card card)
    {
        hoveredCard = card;
    }

    public void CardPointerExit(Card card)
    {
        hoveredCard = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            if (hoveredCard != null)
            {
                Destroy(hoveredCard.transform.parent.gameObject);
                cards.Remove(hoveredCard);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            foreach (Card card in cards)
            {
                card.Deselect();
            }
        }

        if (selectedCard == null)
            return;

        if (isCrossing)
            return;

        for (int i = 0; i < cards.Count; i++)
        {
            if (cards[i].Equals(null))
                return;

            if (selectedCard.transform.position.x > cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() < cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }

            if (selectedCard.transform.position.x < cards[i].transform.position.x)
            {
                if (selectedCard.ParentIndex() > cards[i].ParentIndex())
                {
                    Swap(i);
                    break;
                }
            }
        }
    }

    void Swap(int index)
    {
        isCrossing = true;

        Transform focusedParent = selectedCard.transform.parent;
        Transform crossedParent = cards[index].transform.parent;

        cards[index].transform.SetParent(focusedParent);
        cards[index].transform.localPosition = cards[index].selected ? new Vector3(0, cards[index].selectionOffset, 0) : Vector3.zero;
        selectedCard.transform.SetParent(crossedParent);

        isCrossing = false;

        if (cards[index].cardVisual == null)
            return;

        bool swapIsRight = cards[index].ParentIndex() > selectedCard.ParentIndex();
        cards[index].cardVisual.Swap(swapIsRight ? -1 : 1);

        //Updated Visual Indexes
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }

        //  Refresh selected cards list after swapping
        if(deleteScript == null)
        {
            deleteScript = FindFirstObjectByType<DeleteCard>();
        }
        deleteScript.SortSelectedCards();
    }

    //  Refresh visual index and update playerHand
    public void RefreshVisual()
    {
        cards = GetComponentsInChildren<Card>().ToList(); //  Refresh card list
        deck.playerHand = cards.Select(card => card.pcard).ToList();

        StartCoroutine(Frame());

        IEnumerator Frame()
        {
            yield return new WaitForSecondsRealtime(.1f);
            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardVisual != null)
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
            }
        }
    }

    //  Need to add a little wait when card slots are destroyed before refreshing
    public void DestroyCardsRefresh()
    {
        StartCoroutine(WaitFrame());

        IEnumerator WaitFrame()
        {
            yield return null;

            cards = GetComponentsInChildren<Card>().ToList(); //  Refresh card list
            deck.playerHand = cards.Select(card => card.pcard).ToList();

            yield return null;

            for (int i = 0; i < cards.Count; i++)
            {
                if (cards[i].cardVisual != null)
                    cards[i].cardVisual.UpdateIndex(transform.childCount);
            }
        }
    }

    //  Destroy slots if existing at new start of round (call before DrawHand)
    public void DestroyExistingSlots()
    {
        cards = GetComponentsInChildren<Card>().ToList();
        if (cards == null)
        {
            return;
        }

        foreach (Card cardObject in cards)
        {
            Transform parentSlot = cardObject.transform.parent;

            Destroy(cardObject.gameObject);      // Destroy the card
            if (parentSlot != null)
                Destroy(parentSlot.gameObject);  // Destroy the parent slot
        }

        cards.Clear();
    }

    //  Sets the disabled suit for Special Blinds that disable cards with a certain suit
    public void SetDisabledSuit(bool disabledSuitFlag, SuitName disabledSuitName)
    {
        this.disabledSuitFlag = disabledSuitFlag;

        if(disabledSuitFlag)
        {
            this.disabledSuitName = disabledSuitName;
        }
    }
}
