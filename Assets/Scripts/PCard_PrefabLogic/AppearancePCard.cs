using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class AppearancePCard : MonoBehaviour
{
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
            enhancementLayer.color = new Color(1f, 1f, 1f, 1f); // fully transparent
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
            suitLayer.color = new Color(1f, 1f, 1f, 1f); // fully transparent
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
            sealLayer.color = new Color(1f, 1f, 1f, 1f); // fully transparent
        }
    }
}
