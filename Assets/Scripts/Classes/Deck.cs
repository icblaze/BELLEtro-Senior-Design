// This Document contains the code for the Deck class
// This class contains a set of Cards that the player owns and has functions
// that will effect these cards
// Current Devs:
// Robert (momomonkeyman): made class and variables

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

// The Deck will hold the player's cards and will allow for CRUD operations upon
// these cards (and a random drawing function)
public class Deck
{
  private static Deck instance;

  //Singelton for the Deck class
  public static Deck access()
  {
    if (instance == null)
    {
      instance = new Deck();
    }
    return instance;
  }

  public static int deckSize = 56;  // This variable holds the original deck size
  public static int counter = 0;  

  public List<PCard> deckCards = new List<PCard>(); //This will hold all of the cards that have the card info attach to them.
  public List<PCard> cardsDrawn = new List<PCard>(); //This should store all the cards that were drawn
  public List<PCard> playerHand = new List<PCard>(); //This variable will hold the hand of the player.


  //This constructor sets up the initial deck in the game
  public Deck()
  {
    createDeck(); //Call createDeck to create the deck for the game.
  }

  public bool IsDiphthong(SuitName suit, PlaceArticulation placeArt, MannerArticulation mannerArt, LinguisticTerms term)
  {
    return term.ToString().Contains(suit.ToString()) &&
           term.ToString().Contains(placeArt.ToString()) &&
           (term.ToString().Contains("High") || term.ToString().Contains("Mid") || term.ToString().Contains("Low")) &&
           term.ToString().Contains("Diphthong");
  }
  public bool IsVowel(SuitName suit, PlaceArticulation placeArt, MannerArticulation mannerArt, LinguisticTerms term)
  {
    return term.ToString().Contains(suit.ToString()) &&
           term.ToString().Contains(placeArt.ToString()) &&
           (term.ToString().Contains("High") || term.ToString().Contains("Mid") || term.ToString().Contains("Low"));

  }
  public bool IsValidCombination(SuitName suit, PlaceArticulation placeArt, MannerArticulation mannerArt, LinguisticTerms term)
  {
    if (suit.ToString() == "None")
    {
      return false;
    }

    return term.ToString().Contains(suit.ToString()) &&
             term.ToString().Contains(placeArt.ToString()) &&
             term.ToString().Contains(mannerArt.ToString());
  }

  //This function is responsible for creating a new deck , and removing all the cards from the last deck.
  //This function will be useful when we need to create a new deck for a new round.
  public void createDeck()
  {
    deckCards.Clear();

    foreach (SuitName suit in Enum.GetValues(typeof(SuitName)))
    {
      foreach (PlaceArticulation placeArticulation in Enum.GetValues(typeof(PlaceArticulation)))
      {
        foreach (MannerArticulation mannerArticulation in Enum.GetValues(typeof(MannerArticulation)))
        {
          foreach (LinguisticTerms linguisticterm in Enum.GetValues(typeof(LinguisticTerms)))
          {
            if (IsValidCombination(suit, placeArticulation, mannerArticulation, linguisticterm) == true)
            {
              PCard newCard = new PCard
              {
                kindOfCard = CardType.Card,
                term = linguisticterm,
                suit = suit,
                placeArt = placeArticulation,
                mannerArt = mannerArticulation,
                isDiphthong = false,
                chips = 10,
                multiplier = 1,
                edition = CardEdition.Base,
                enhancement = CardEnhancement.Base,
                seal = CardSeal.Base,
                isDisabled = false,
              };

              counter++;

              if (IsVowel(suit, placeArticulation, mannerArticulation, linguisticterm) == true)
              {
                newCard.chips = 8;

                if (IsDiphthong(suit, placeArticulation, mannerArticulation, linguisticterm) == true)
                {
                  newCard.isDiphthong = true;
                }
                deckCards.Add(new PCard
                {
                  kindOfCard = CardType.Card,
                  term = linguisticterm,
                  suit = suit,
                  placeArt = placeArticulation,
                  mannerArt = mannerArticulation,
                  isDiphthong = newCard.isDiphthong,
                  chips = 8,
                  multiplier = 1,
                  edition = CardEdition.Base,
                  enhancement = CardEnhancement.Base,
                  seal = CardSeal.Base,
                  isDisabled = false,
                });
                counter++;
                Debug.LogWarning($"Card {counter}: {newCard.kindOfCard}, {newCard.term}, {newCard.suit}, {newCard.placeArt}, {newCard.mannerArt}, Diphthong: {newCard.isDiphthong}, Chips: {newCard.chips}");

              }
              deckCards.Add(newCard);
              Debug.LogWarning($"Card {counter}: {newCard.kindOfCard}, {newCard.term}, {newCard.suit}, {newCard.placeArt}, {newCard.mannerArt}, Diphthong: {newCard.isDiphthong}, Chips: {newCard.chips}");

            }
          }
        }
      }

    }
    counter = 0; //Set the counter back to 0
  }

  //This function is responsible for drawing a certain amount of cards into the players hand.
  //PlayerHandCount represents how many missing cards are missing from the players hand.
  public PCard[] drawCards(Game game, int playerHandCount)
  {
    if (deckCards.Count == 0)
    {
      return null;
    }

    //This calls the random draw function with the entire deck and playerhandcount
    PCard[] list = game.randomDraw(deckCards, playerHandCount);

    //Remove cards from the deck
    for (int i = 0; i < playerHandCount; i++)
    {
      PCard removed = removeCard(list[i]);
      if (removed != null)
      {
        cardsDrawn.Add(removed);
      }

    }

    return list;
  }

  //Adds a card to the deck
  public void addCard(PCard card)
  {
    if (card != null)
    {
      deckCards.Add(card);
    }
  }

  //This function should remove the card from the deck and return the removed card.  
  public PCard removeCard(PCard card)
  {
    if (deckCards.Remove(card))
    {
      PCard removedCard = card;
      return removedCard;
    }

    return null;
  }

  //This function is responsible for updating the card selected.
  //Update the card in the scene
  //Might need to return the update card so that we can attach it to the gameobject
  public void updateCard(PCard originalCard, PCard updatedCard)
  {
    int index = deckCards.FindIndex(card => card == originalCard);

    if (index != -1)
    {
      deckCards[index] = updatedCard;
    }

  }

}
