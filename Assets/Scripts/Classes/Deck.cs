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
  public static int deckSize = 56;  //Deck count

  //Constructor for the Deck so that we can create our deck for the game.
  public Deck()
  {
   
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
