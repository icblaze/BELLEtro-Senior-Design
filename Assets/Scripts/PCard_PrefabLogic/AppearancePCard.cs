//This class handles putting the correct images/data on each card in the game.
//This class is used to add the correct visual aspects for a card in the game.
//Current Devs:
//Andy Van

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AppearancePCard : MonoBehaviour
{
    [SerializeField] private Image baseLayer;
    [SerializeField] private Image suitLayer;
    [SerializeField] private TextMeshProUGUI termLayer;
    [SerializeField] private Image sealLayer;
    [SerializeField] private Image enhancementLayer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


    }

    //  When Enhancement of PCard is updated, change layer
    public void UpdateEnhancement(string enhancement)
    {
        Sprite sprite = Resources.Load<Sprite>($"PCard/Enhancements/" + enhancement);

        if (sprite == null)
        {
            enhancementLayer.color = new Color(1f, 1f, 1f, 0f); // fully transparent
        }
        else
        {
            enhancementLayer.sprite = sprite;
            enhancementLayer.color = new Color(1f, 1f, 1f, 1f); // fully opaque
        }
    }

    //  When Suit of PCard is updated, change layer
    public void UpdateSuit(string suit)
    {
        Sprite sprite = Resources.Load<Sprite>($"PCard/Suits/" + suit);

        if (sprite == null)
        {
            suitLayer.color = new Color(1f, 1f, 1f, 0f); // fully transparent
        }
        else
        {
            suitLayer.sprite = sprite;
            suitLayer.color = new Color(1f, 1f, 1f, 1f); // fully opaque
        }
    }

    //  When Term of PCard is updated, change layer
    public void UpdateTerm(string termSymbol)
    {
        termLayer.text = termSymbol;
    }

    //  When Seal of PCard is updated, change layer
    public void UpdateSeal(string seal)
    {
        Sprite sprite = Resources.Load<Sprite>($"PCard/Seals/" + seal);

        if (sprite == null)
        {
            sealLayer.color = new Color(1f, 1f, 1f, 0f); // fully transparent
        }
        else
        {
            sealLayer.sprite = sprite;
            sealLayer.color = new Color(1f, 1f, 1f, 1f); // fully opaque
        }
    }

    //  Update base layer for Mentor, disable other layers
    public void UpdateMentor(string mentorName)
    {
        baseLayer.sprite = Resources.Load<Sprite>($"Mentor/" + mentorName);

        //  Hide other layers since not relevant to this card type
        suitLayer.color = new Color(1f, 1f, 1f, 0f);
        sealLayer.color = new Color(1f, 1f, 1f, 0f); 
        enhancementLayer.color = new Color(1f, 1f, 1f, 0f);
        termLayer.text = "";
    }

    //  Update base layer for Consumable, disable other layers
    public void UpdateConsumable(ConsumableType type, string consumableName)
    {
        //  Get sprite from appropiate folder
        if(type == ConsumableType.CardBuff)
        {
            baseLayer.sprite = Resources.Load<Sprite>($"CardBuff/food_" + consumableName);
        }
        else
        {
            baseLayer.sprite = Resources.Load<Sprite>($"Textbook/textbook_" + consumableName);
        }

        //  Hide other layers since not relevant to this card type
        suitLayer.color = new Color(1f, 1f, 1f, 0f);
        sealLayer.color = new Color(1f, 1f, 1f, 0f);
        enhancementLayer.color = new Color(1f, 1f, 1f, 0f);
        termLayer.text = "";
    }
}
