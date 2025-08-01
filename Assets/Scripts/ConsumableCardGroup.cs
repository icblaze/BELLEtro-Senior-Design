// This Document contains the code for managing the player's consumables list visually.
// This includes the logic for consumables rearaangement and using effect.
// Current Devs:
// Van: created the class and sell button
// Andy: linked Card visual with player's consumables list, connected use button to effect

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

    [Header("Test Mode")]
    public bool testMode = true; // Spawn random cards for testing

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    //  DeleteCard to update current hand if cards changed
    [SerializeField] private DeleteCard deleteScript;

    Player player = Player.access();
    Game game = Game.access();

    void Start()
    {
        if (testMode)
        {
            player.consumables.Add(CardBuff.CardBuffFactory(CardBuffName.Cherry));
            player.consumables.Add(new Textbook(TextbookName.HighCard));
        }

        //  Debug consumables in the list, order from left to right
        Debug.Log("Consumables in list:");
        foreach (Consumable consumable in player.consumables)
        {
            //  Realize that it might be better to just have the name be in Consumable
            if (consumable.type == ConsumableType.Textbook)
            {
                Textbook tbook = (Textbook)consumable;
                Debug.Log($"Textbook: {tbook.name}");
            }
            else
            {
                CardBuff cardBuff = (CardBuff)consumable;
                Debug.Log($"Card Buff: {cardBuff.name}");
            }
        }

        //  Create slots for each consumable in the player's list
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

    //This function is called when a card is hovered over, and it will update the hoveredCard variable.
    void CardPointerEnter(Card card)
    {
        hoveredCard = card;

        //  Update interactable status of use button
        if (cardToUse != null)
        {
            if (card.consumable.type == ConsumableType.CardBuff && cardToUse.Equals(card))
            {
                CardBuff cardBuff = (CardBuff)card.consumable;
                Button useBtn = currentUseButton.GetComponent<Button>();

                useBtn.interactable = !cardBuff.CheckDisabled();
            }
        }
    }

    //This function is called when a card is not longer hovered over, and it will reset the hoveredCard variable.
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
                //This destroy will remove the game object of the hovered card and its parent slot.
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


    //This function is called when a card is clicked, and it will create a sell button and a use button for the card.
    void OnCardClicked(Card clickedCard)
    {
        // Check if we are clicking the same card again to deselect it.
        // We can use cardToSell as the reference for the currently selected card.
        if (cardToSell == clickedCard)
        {
            // If yes, destroy both buttons and clear the references.
            if (currentSellButton != null)
            {
                Destroy(currentSellButton);
            }
            if (currentUseButton != null)
            {
                Destroy(currentUseButton);
            }
            cardToSell = null;
            cardToUse = null;
            return; // Exit the function, we are done.
        }

        // If we are here, it means we clicked a NEW card.

        // First, clean up any old buttons from a previously selected card.
        if (currentSellButton != null)
        {
            Destroy(currentSellButton);
        }
        if (currentUseButton != null)
        {
            Destroy(currentUseButton);
        }

        // Now, create the new buttons on the newly clicked card.

        // --- Create Sell Button ---
        cardToSell = clickedCard;
        currentSellButton = Instantiate(sellButtonPrefab, clickedCard.transform);
        currentSellButton.transform.localPosition = new Vector3(0, 150, 0);
        currentSellButton.GetComponentInChildren<TMP_Text>().text = "Sell $" + cardToSell.consumable.sellValue.ToString();
        Button sellBtn = currentSellButton.GetComponent<Button>();
        sellBtn.onClick.AddListener(SellCard);

        // --- Create Use Button ---
        cardToUse = clickedCard;
        currentUseButton = Instantiate(useButtonPrefab, clickedCard.transform);
        currentUseButton.transform.localPosition = new Vector3(0, -150, 0);
        Button useBtn = currentUseButton.GetComponent<Button>();
        useBtn.onClick.AddListener(UseCard);

        // Check interactability status if it's a Card Buff
        if (clickedCard.consumable.type == ConsumableType.CardBuff)
        {
            CardBuff cardBuff = (CardBuff)clickedCard.consumable;
            useBtn.interactable = !cardBuff.CheckDisabled();
        }
    }

    public void UseCard()
    {
        if (cardToUse != null)
        {
            //  Hide the description
            cardToUse.HideDescription();

            if (deleteScript == null)
            {
                deleteScript = FindFirstObjectByType<DeleteCard>();
            }

            //  Use effect
            if (cardToUse.consumable.type == ConsumableType.Textbook)
            {
                Textbook tbook = (Textbook)cardToUse.consumable;
                tbook.applyTextbook();
            }
            else
            {
                CardBuff cardBuff = (CardBuff)cardToUse.consumable;
                cardBuff.applyCardBuff();

                //  Refresh cards if possible hand changed
                if (!cardBuff.isInstant)
                {
                    CurrentHandManager.Instance.findCurrentHand(deleteScript.GetSelectedPCards());
                }
            }

            SFXManager sfxManager = GameObject.FindFirstObjectByType<SFXManager>().GetComponent<SFXManager>();
            sfxManager.UseButton();

            //  Remove consumable from player's consuamble list first
            player.consumables.Remove(cardToUse.consumable);

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

            //  Hide the description
            cardToSell.HideDescription();

            // Make sure everything exists before proceeding
            if (player != null && cardToSell != null && shopManager != null)
            {
                Debug.Log("--- Selling Card ---");
                Debug.Log("Money BEFORE sale: " + player.moneyCount);
                Debug.Log("Card sell value: " + cardToSell.consumable.sellValue);

                // Add the card's value to the player's money
                player.moneyCount += cardToSell.consumable.sellValue;

                Debug.Log("Money AFTER sale: " + player.moneyCount);

                SFXManager sfxManager = GameObject.FindFirstObjectByType<SFXManager>().GetComponent<SFXManager>();
                sfxManager.UseButton();

                // Tell the ShopManager to update the UI display
                shopManager.UpdateMoneyDisplay();
            }

            // Remove the card from our list, associated slot, and destroy its game object

            //  Remove consumable from player's consuamble list first
            player.consumables.Remove(cardToSell.consumable);

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
