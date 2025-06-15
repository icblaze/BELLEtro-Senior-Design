// This class will help us link the card data to a card object inside the game.
// Current Devs:
// Zacharia Alaoui : Made class

using System;
using UnityEngine;

public class CardObject : MonoBehaviour
{
    public PCard cardData; //Stores card info 

    public Sprite cardSprite; //This will allow us to assign a sprite to the card for visual representation

    // Sets the data for the CardObject
    public void Init(PCard data, Sprite sprite = null)
    {
        cardData = data;
        cardSprite = sprite;
    }
}