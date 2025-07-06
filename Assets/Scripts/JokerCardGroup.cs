// This Document contains the code for managing the player's mentorDeck visually.
// This includes the logic for mentor rearaangement.
// Current Devs:
// Van: created the class and sell button
// Andy: linked Card visual and player's mentorDeck

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using UnityEngine.UI;

public class JokerCardHolder : MonoBehaviour
{
    [SerializeField] private Card selectedCard;
    [SerializeReference] private Card hoveredCard;

    [SerializeField] private GameObject slotPrefab;
    private RectTransform rect;

    [Header("Mentor List")]
    public List<Card> cards;

    [Header("Sell Button")]
    [SerializeField] private GameObject sellButtonPrefab; // Holds the button template
    private GameObject currentSellButton; // The active sell button in the scene
    [SerializeField] private Card cardToSell; // The card we want to sell

    bool isCrossing = false;
    [SerializeField] private bool tweenCardReturn = true;

    Player player = Player.access();
    Game game = Game.access();

    void Start()
    {
        //  Add 1 random mentor at each rerun of scene for testing purposes!
        //  Order should persist if correct, selling should remove from mentorDeck  
        //  Comment out eventually
        player.mentorDeck.Add(Mentor.MentorFactory(MentorName.BonusPoints, CardEdition.Holographic));
        player.mentorDeck.Add(Mentor.MentorFactory(MentorName.Astronaut, CardEdition.Polychrome));

        //  Debug mentors in the list, order from left to right
        int count = 1;
        Debug.Log("Mentors in list:\n");
        foreach (Mentor mentor in player.mentorDeck)
        {
            Debug.Log("Mentor " + count + ": " + mentor.name.ToString());
            count++;
        }

        for (int i = 0; i < player.mentorDeck.Count; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, transform);
            newSlot.name = $"MentorSlot {i + 1}"; // Assign meaningful names
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
            card.AssignMentor(player.mentorDeck[cardCount]);
            card.name = card.mentor.name.ToString(); 
            cardCount++;
        }

        StartCoroutine(RefreshFrame());
    }

    IEnumerator RefreshFrame()
    {
        yield return new WaitForSecondsRealtime(.1f);
        RefreshMentors();
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

        //  When mentors get rearranged, reassign mentor buffers, change mentorDeck
        RefreshMentors();
    }

    void CardPointerEnter(Card card)
    {
        hoveredCard = card;
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

        currentSellButton.GetComponentInChildren<TMP_Text>().text = "Sell $" + cardToSell.GetComponent<Card>().mentor.sellValue.ToString();
        Button sellBtn = currentSellButton.GetComponent<Button>();
        sellBtn.onClick.AddListener(SellCard);
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
                Debug.Log("Card sell value: " + cardComponent.mentor.sellValue);

                // Add the card's value to the player's money
                player.moneyCount += cardComponent.mentor.sellValue;

                Debug.Log("Money AFTER sale: " + player.moneyCount);

                // Tell the ShopManager to update the UI display
                shopManager.UpdateMoneyDisplay();
            }

            // Remove the card from our list, associated slot, and destroy its game object

            //  Remove mentor from player's mentor deck first
            player.mentorDeck.Remove(cardComponent.mentor);

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

    //  Visually update mentor deck when added
    public void AddMentor(Mentor newMentor)
    {
        //  Instatiate a new slot
        GameObject newSlot = Instantiate(slotPrefab, transform);
        newSlot.name = $"New MentorSlot";

        //  Assign mentor to card in new slot
        Card card = newSlot.GetComponentInChildren<Card>();

        card.PointerEnterEvent.AddListener(CardPointerEnter);
        card.PointerExitEvent.AddListener(CardPointerExit);
        card.BeginDragEvent.AddListener(BeginDrag);
        card.EndDragEvent.AddListener(EndDrag);
        card.PointerClickEvent.AddListener(OnCardClicked);  //  For sell function
        card.AssignMentor(newMentor);
        card.name = card.mentor.name.ToString();

        //  Refresh mentor list
        StartCoroutine(RefreshFrame());
    }

    //  When mentors get rearranged, reassign mentor buffers, change mentorDeck
    void RefreshMentors()
    {
        cards = GetComponentsInChildren<Card>().ToList();
        player.mentorDeck = cards.Select(card => card.mentor).ToList();

        //  Some mentors rely on position to copy effect
        foreach (Mentor mentor in player.mentorDeck)
        {
            if(mentor.name == MentorName.CheatSheet)
            {
                CheatSheet cheatSheet = (CheatSheet) mentor;
                cheatSheet.ChangeEffect();
            }
            else if(mentor.name == MentorName.Brainstorm)
            {
                Brainstorm brainstorm = (Brainstorm) mentor;
                brainstorm.ChangeEffect();
            }
        }

        MentorBufferManager.access().AssignToBuffer();
        foreach (Card card in cards)
        {
            card.cardVisual.UpdateIndex(transform.childCount);
        }
    }
}
