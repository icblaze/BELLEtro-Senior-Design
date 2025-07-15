using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class DeleteCard : MonoBehaviour
{
    [SerializeField] private Button deleteButton;           //Delete button
    [SerializeField] private Transform playingCardGroup;    //Transform 
    [HideInInspector] public TextMeshProUGUI discardsLeft;
    [HideInInspector] private static int discardCount;
    private DeckManager deckManager;                        //Instance of the Deck Manager
    [SerializeField] private List<GameObject> selectedCards = new List<GameObject>();        //List of the current Gameobjects that the user has selected
    private List<PCard> selectedPCards = new List<PCard>();                 //List of the selected cards that the user has selected
    [SerializeField] private int pcardCount;
    private CardType cardType;

    public bool cashPenalty = false;
    public bool drawThree = false;

    void Start()
    {
        deckManager = FindFirstObjectByType<DeckManager>();
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
        {
            discardCount = Player.access().maxDiscards;
            discardsLeft = GameObject.Find("Discards Number Text").GetComponent<TextMeshProUGUI>();
            deleteButton.onClick.AddListener(RemoveSelectedCards);
        }
        else
        {
            Debug.LogError("DeleteCard: deleteButton not assigned in Inspector!");
        }
    }

    //  Sets discards to be the maxDiscards after blind select or end of round
    public void ResetDiscards()
    {
        Player.access().discards = Player.access().maxDiscards;
        discardCount = Player.access().discards;
        discardsLeft.text = discardCount.ToString();
    }

    //  Sets discards after reset (by Mentors
    public void SetDiscards(int discards)
    {
        Player.access().discards = discards;
        discardCount = Player.access().discards;
        discardsLeft.text = discardCount.ToString();
    }

    //Adds the card to the selectedCards list
    public void AddSelectedCard(GameObject card)
    {
        if (card == null) return;

        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent == null || cardComponent.cardType != CardType.Card) return;


        if (!selectedCards.Contains(card))
        {
            selectedCards.Add(card);

            //  Sort selected cards to match left to right order of the hand
            selectedCards.Sort((a, b) =>
            {
                int aOrder = a.GetComponent<Card>().ParentIndex();
                int bOrder = b.GetComponent<Card>().ParentIndex();
                return aOrder.CompareTo(bOrder);
            });

            //  Debug hand check
            CurrentHandManager.Instance.findCurrentHand(GetSelectedPCards());
            pcardCount = selectedPCards.Count;
        }
    }

    //This removes the card the player unselected from the selectedCards list
    public void RemoveSelectedCard(GameObject card)
    {
        if (card == null) return;

        Card cardComponent = card.GetComponent<Card>();
        if (cardComponent == null || cardComponent.cardType != CardType.Card) return;


        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);

            //  Debug hand check
            CurrentHandManager.Instance.findCurrentHand(GetSelectedPCards());
            pcardCount = selectedPCards.Count;

        }
    }

    //This returns the selected cards that the player currently has selected
    public List<GameObject> GetSelectedCards()
    {
        return new List<GameObject>(selectedCards);
    }

    //This returns the selected PCards extracted from cards that the player currently has selected
    public List<PCard> GetSelectedPCards()
    {
        selectedPCards.Clear(); //  Refresh PCards

        foreach (GameObject cardObject in selectedCards)
        {
            selectedPCards.Add(cardObject.GetComponent<Card>().pcard);
        }

        return new List<PCard>(selectedPCards);
    }

    //This function removes the cards that the player played, and clears the selectedCards list.
    //This function also starts a coroutine to fill the player hand with new cards
    public void RemoveSelectedCards()
    {
        if (selectedCards.Count > 5)
        {
            Debug.LogWarning("You cannot discard more than 5 cards at a time!");
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            // Stop the function here so no cards are deleted.
            return;
        }
        if (selectedCards.Count == 0)
        {
            Debug.LogWarning("No cards selected for deletion!");
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            SFXManager sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
            sfxManager.NoSFX();
            return;
        }

        if (discardCount == 0)
        {
            Debug.LogWarning("You don't have anymore discards left!");
            ShakeScreen shakeScreen = FindFirstObjectByType<ShakeScreen>().GetComponent<ShakeScreen>();
            shakeScreen.StartShake();
            SFXManager sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
            sfxManager.NoSFX();

            return;
        }

        //  Activate on discard
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Discard);

        PCard[] newCards;
        if (drawThree == true)//If special blind is active, draw three
        {
            newCards = Deck.access().drawCards(3);
        }
        else
        {
            //  Draw from deck equal to amount played
            newCards = Deck.access().drawCards(selectedCards.Count);
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
            RemoveSelectedCard(card);
            Destroy(card);
        }

        discardCount = Round.access().DecreaseDiscardCount();      //Decrease Hand count of the player
        discardsLeft.text = discardCount.ToString("0");

        if (cashPenalty)
        {
            Player.access().moneyCount--;
            ShopManager.access().UpdateMoneyDisplay();
            SFXManager sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
            sfxManager.MoneyUsed();
        }

        StartCoroutine(RefillNextFrame(newCards));
    }

    private IEnumerator RefillNextFrame(PCard[] newCards)
    {
        // wait one frame so the Slots are truly empty
        yield return null;

        //  Clean up extra slots if effects that given it have been sold/used up
        int extraSlots = deckManager.GetSlotCount() - Player.access().handSize;

        while (extraSlots > 0)
        {
            Transform emptySlot = deckManager.GetFirstEmptySlot();
            if (emptySlot != null)
            {
                Destroy(emptySlot.gameObject);
            }
            extraSlots--;
            yield return null;
        }

        // here’s the only change: use DrawCard() so it honors your Slot layout
        for (int i = 0; i < newCards.Length; i++)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            deckManager.DrawCard(newCards[i]);
        }

        int missingSlots;

        if (drawThree == true)//If special blind is active, ignore handSize
        {
            missingSlots = 3;
        }
        else
        {
            missingSlots = Player.access().handSize - deckManager.GetSlotCount();
        }
        if (missingSlots > 0)
        {
            PCard[] extraCards = Deck.access().drawCards(missingSlots);

            foreach (PCard extra in extraCards)
            {
                deckManager.DrawNewSlot(extra);
            }
        }
    }
}
