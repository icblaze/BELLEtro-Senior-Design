// This Document contains the code for the Deck class
// This class contains a set of Cards that the player owns and has functions
// that will effect these cards
// Current Devs:
// Robert (momomonkeyman): made class and variables

using System.Collections;
using UnityEngine;

// The Deck will hold the player's cards and will allow for CRUD operations upon
// these cards (and a random drawing function)
public class Deck
{
  public PCard[] cards;

  public PCard[] drawCards(Game game)
  {
    PCard card = new PCard();
    PCard[] list = { card };
    return list;
  }

  public void addCard (PCard card)
  {

  }

  public PCard removeCard (PCard card)
  {
    PCard card2 = new PCard();
    return card2;
  }

  public void updateCard (PCard original, PCard updated)
  {

  }
}
