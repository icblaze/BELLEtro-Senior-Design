using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DeleteCard : MonoBehaviour
{
    [SerializeField] private Button deleteButton;           //Delete button
    [SerializeField] private Transform playingCardGroup;    //Transform 
    [HideInInspector] private TextMeshProUGUI discardsLeft;
    [HideInInspector] private static int discardCount;
    private DeckManager deckManager;                        //Instance of the Deck Manager
    private List<GameObject> selectedCards = new List<GameObject>();        //List of the current Gameobjects that the user has selected
    private List<PCard> selectedPCards = new List<PCard>();                 //List of the selected cards that the user has selected
    CurrentHandManager currentHandManager = new CurrentHandManager();       //  For detecting current hand

    //  Access to Round
    Round round = Round.access();

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
            discardCount = Player.access().discards;
            discardsLeft = GameObject.Find("Discards Number Text").GetComponent<TextMeshProUGUI>();
            deleteButton.onClick.AddListener(RemoveSelectedCards);
        }
        else
        {
            Debug.LogError("DeleteCard: deleteButton not assigned in Inspector!");
        }
    }

    //Adds the card to the selectedCards list
    public void AddSelectedCard(GameObject card)
    {
        if (card == null || card.GetComponent<Card>().cardType != CardType.Card) return;
        if (!selectedCards.Contains(card))
        {
            //  Extract PCard object from Card
            PCard pcard = card.GetComponent<Card>().pcard;
            selectedPCards.Add(pcard);

            selectedCards.Add(card);

            //  Debug hand check
            Debug.Log(selectedPCards.Count);
            //Debug.Log(currentHandManager.findCurrentHand(selectedPCards));
        }
    }

    //This removes the card the player unselected from the selectedCards list
    public void RemoveSelectedCard(GameObject card)
    {
        if (card == null || card.GetComponent<Card>().cardType != CardType.Card) return;
        if (selectedCards.Contains(card))
        {
            //  Extract PCard object from Card
            PCard pcard = card.GetComponent<Card>().pcard;
            selectedPCards.Remove(pcard);

            selectedCards.Remove(card);

            //  Debug hand check
            Debug.Log(selectedPCards.Count);
            //Debug.Log(currentHandManager.findCurrentHand(selectedPCards));
        }
    }

    //This returns the selected cards that the player currently has selected
    public List<GameObject> GetSelectedCards()
    {
        return new List<GameObject>(selectedCards);
    }

    //This returns the selected PCards that the player currently has selected
    public List<PCard> GetSelectedPCards()
    {
        return new List<PCard>(selectedPCards);
    }

    //This function removes the cards that the player played, and clears the selectedCards list.
    //This function also starts a coroutine to fill the player hand with new cards
    public void RemoveSelectedCards()
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

        if (discardCount == 0)
        {
            Debug.LogWarning("You don't have anymore discards left!");
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
        
        discardCount = Round.access().DecreaseDiscardCount();      //Decrease Hand count of the player
        discardsLeft.text = discardCount.ToString("0");


        selectedPCards.Clear(); //  Clear PCard list as well
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
