using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;

public class ConsumableCardHolder : MonoBehaviour
{
    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Consumable Card List")]
    public List<Card> cards;

    [Header("Sell Button")]
    [SerializeField] private GameObject sellButtonPrefab; // Holds the sell button template
    private GameObject currentSellButton; // The active sell button in the scene
    [SerializeField] private Card cardToSell; // The card we want to sell

    [Header("Use Button")]
    [SerializeField] private GameObject useButtonPrefab; // Holds the use button template
    private GameObject currentUseButton; // The active use button in the scene
    [SerializeField] private Card cardToUse; // The card we want to use

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    Player player = Player.access();
    Game game = Game.access();

    void Start()
    {
        //  Add 2 consumable at each rerun of scene for testing purposes!
        //  Order should persist if correct, selling should remove from consumables  
        //  Comment out eventually
        player.consumables.Add(CardBuff.CardBuffFactory(CardBuffName.Leftovers));
        player.consumables.Add(CardBuff.CardBuffFactory(CardBuffName.Tea));

        //  Debug consumables in the list, order from left to right
        Debug.Log("Consumables in list:");
        foreach (Consumable consumable in player.consumables)
        {
            //  Realize that it might be better to just have the name be in Consumable
            if(consumable.type == ConsumableType.Textbook)
            {
                Textbook tbook = (Textbook) consumable;
                Debug.Log($"Textbook: {tbook.name}");
            }
            else
            {
                CardBuff cardBuff = (CardBuff) consumable;
                Debug.Log($"Card Buff: {cardBuff.name}");
            }
        }

        for (int i = 0; i < player.consumables.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform);
            newSlot.name = $"ConsumableSlot {i + 1}"; // Assign meaningful names
        }

        rect = GetComponent<RectTransform>();
        cards = GetComponentsInChildren<Card>().ToList();

        int cardCount = 0;
        foreach (Card card in cards)
        {
            card.PointerEnterEvent.AddListener(CardPointerEnter);
            card.PointerExitEvent.AddListener(CardPointerExit);
            card.BeginDragEvent.AddListener(BeginDrag);
            card.EndDragEvent.AddListener(EndDrag);
            card.PointerClickEvent.AddListener(OnCardClicked);  //  For sell function
            card.AssignConsumable(player.consumables[cardCount]);
            card.name = card.consumableName;
            cardCount++;
        }

        StartCoroutine(RefreshFrame());
    }

    IEnumerator RefreshFrame()
    {
        yield return new WaitForSecondsRealtime(.1f);
        RefreshConsumables();
    }

    private void BeginDrag(Card card)
    {
        selectedCard = card;
    }

    void EndDrag(Card card)
    {
        if (selectedCard == null)
            return;

        selectedCard.transform.DOLocalMove(selectedCard.selected ? new Vector3(0, selectedCard.selectionOffset, 0) : Vector3.zero, tweenCardReturn ? .15f : 0).SetEase(Ease.OutBack);

        rect.sizeDelta += Vector2.right;
        rect.sizeDelta -= Vector2.right;

        selectedCard = null;

        //  When consumables get rearranged, update consumable list
        RefreshConsumables();
    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;

        //  Update interactable status of use button
        if (cardToUse != null)
        {
            if(card.consumable.type == ConsumableType.CardBuff && cardToUse.Equals(card))
            {
                CardBuff cardBuff = (CardBuff)card.consumable;
                Button useBtn = currentUseButton.GetComponent<Button>();

                useBtn.interactable = !cardBuff.CheckDisabled();
            }
        }
    }

    void CardPointerExit(Card card)
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
    }

    void OnCardClicked(Card clickedCard)
    {
        if (currentSellButton != null)
        {
            Destroy(currentSellButton);
        }

        cardToSell = clickedCard;
        currentSellButton = Instantiate(sellButtonPrefab, clickedCard.transform);

        currentSellButton.transform.localPosition = new Vector3(0, 150, 0);
        // This resets the button's position to the center of the card.

        currentSellButton.GetComponentInChildren<TMP_Text>().text = "Sell $" + cardToSell.GetComponent<Card>().consumable.sellValue.ToString();
        Button sellBtn = currentSellButton.GetComponent<Button>();
        sellBtn.onClick.AddListener(SellCard);

        //  For use consumables
        if (currentUseButton != null)
        {
            Destroy(currentUseButton);
        }

        cardToUse = clickedCard;
        currentUseButton = Instantiate(useButtonPrefab, clickedCard.transform);

        currentUseButton.transform.localPosition = new Vector3(0, -150, 0);
        Button useBtn = currentUseButton.GetComponent<Button>();
        useBtn.onClick.AddListener(UseCard);

        //  Check interactability status if Card Buff
        if (clickedCard.consumable.type == ConsumableType.CardBuff)
        {
            CardBuff cardBuff = (CardBuff)clickedCard.consumable;
            useBtn.interactable = !cardBuff.CheckDisabled();
        }
    }

    void UseCard()
    {
        if (cardToUse != null)
        {
            // Get the Card component to access its sellValue
            Card cardComponent = cardToUse.GetComponent<Card>();

            //  Use effect
            if (cardComponent.consumable.type == ConsumableType.Textbook)
            {
                Textbook tbook = (Textbook)cardComponent.consumable;
                tbook.applyTextbook();
            }
            else
            {
                CardBuff cardBuff = (CardBuff)cardComponent.consumable;
                cardBuff.applyCardBuff();
            }

            //  Remove consumable from player's consuamble list first
            player.consumables.Remove(cardComponent.consumable);

            cards.Remove(cardToUse);

            Transform parentSlot = cardToUse.transform.parent;

            Destroy(cardToUse.gameObject);      // Destroy the card
            if (parentSlot != null)
                Destroy(parentSlot.gameObject);  // Destroy the parent slot

            // Destroy the sell button itself
            Destroy(currentUseButton);

            // Clear our variables
            cardToUse = null;
            currentUseButton = null;

            //  Wait for GameObject deletion before refreshing
            StartCoroutine(RefreshFrame());
        }
    }

    void SellCard()
    {
        if (cardToSell != null)
        {
            // Get the Player and ShopManager instances
            ShopManager shopManager = ShopManager.access();

            // Get the Card component to access its sellValue
            Card cardComponent = cardToSell.GetComponent<Card>();

            // Make sure everything exists before proceeding
            if (player != null && cardComponent != null && shopManager != null)
            {
                Debug.Log("--- Selling Card ---");
                Debug.Log("Money BEFORE sale: " + player.moneyCount);
                Debug.Log("Card sell value: " + cardComponent.consumable.sellValue);

                // Add the card's value to the player's money
                player.moneyCount += cardComponent.consumable.sellValue;

                Debug.Log("Money AFTER sale: " + player.moneyCount);

                // Tell the ShopManager to update the UI display
                shopManager.UpdateMoneyDisplay();
            }

            // Remove the card from our list, associated slot, and destroy its game object

            //  Remove consumable from player's consuamble list first
            player.consumables.Remove(cardComponent.consumable);

            cards.Remove(cardToSell);

            Transform parentSlot = cardToSell.transform.parent;

            Destroy(cardToSell.gameObject);      // Destroy the card
            if (parentSlot != null)
                Destroy(parentSlot.gameObject);  // Destroy the parent slot

            // Destroy the sell button itself
            Destroy(currentSellButton);

            // Clear our variables
            cardToSell = null;
            currentSellButton = null;

            //  Wait for GameObject deletion before refreshing
            StartCoroutine(RefreshFrame());
        }
    }

    //  Visually update consumable deck when added
    public void AddConsumable(Consumable newConsumable)
    {
        //  Instatiate a new slot
        GameObject newSlot = Instantiate(slotPrefab, transform);
        newSlot.name = $"New ConsumableSlot";

        //  Assign mentor to card in new slot
        Card card = newSlot.GetComponentInChildren<Card>();

        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
        card.PointerClickEvent.AddListener(OnCardClicked);  //  For sell function
        card.AssignConsumable(newConsumable);
        card.name = card.consumableName;

        //  Refresh mentor list
        StartCoroutine(RefreshFrame());
    }

    //  When mentors get rearranged, refresh visual index, and change consumables list of Player
    void RefreshConsumables()
    {
        cards = GetComponentsInChildren<Card>().ToList();
        player.consumables = cards.Select(card => card.consumable).ToList();

        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }
}
