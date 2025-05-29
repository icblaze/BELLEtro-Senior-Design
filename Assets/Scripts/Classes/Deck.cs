// This Document contains the code for the Deck class
// This class contains a set of Cards that the player owns and has functions
// that will effect these cards
// Current Devs:
// Robert (momomonkeyman): made class and variables

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;
using UnityEngine;

// The Deck will hold the player's cards and will allow for CRUD operations upon
// these cards (and a random drawing function)
public class Deck
{
  public static int deckSize = 56;  //Deck count
  public static int counter = 0;

  public List<PCard> deckCardsData = new List<PCard>(); //This will hold all of the cards that have the card info attach to them.

  //This constructor handles the creation of the deck for the game.
  public Deck()
  {
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
                deckCardsData.Add(new PCard
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
              deckCardsData.Add(newCard);
              Debug.LogWarning($"Card {counter}: {newCard.kindOfCard}, {newCard.term}, {newCard.suit}, {newCard.placeArt}, {newCard.mannerArt}, Diphthong: {newCard.isDiphthong}, Chips: {newCard.chips}");

            }
          }
        }
      }

    }
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
    return term.ToString().Contains(suit.ToString()) &&
           term.ToString().Contains(placeArt.ToString()) &&
           term.ToString().Contains(mannerArt.ToString());
  }
  
  public PCard[] cards; //This variable will hold the hand of the player.

  public PCard[] drawCards(Game game, int playerHandCount)
  {
    PCard card = new PCard();
    PCard[] list = { card };
    return list;
  }

  public void addCard(PCard card)
  {

  }

  public PCard removeCard(PCard card)
  {
    PCard card2 = new PCard();
    return card2;
  }

  //This function is responsible for updating the card selected based of a CardType such as a Mentor, Textbook, or CardBuff.
  public void updateCard(PCard original, PCard updated)
  {

  }

}
