using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;
using TMPro;

//Script is used to bring up the card details for cards in packs.
//This is helping to alleviate the size of the ShopManager script.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

public class PackDetailsManager : MonoBehaviour
{
    public CanvasGroup card1Details;
    public CanvasGroup card2Details;
    public CanvasGroup card3Details;
    public CanvasGroup card4Details;
    public CanvasGroup card5Details;
    public ShopManager shopManager;
    public void CardDetails1()
    {
        ShowCardDetails(card1Details, 1);
    }
    public void CardDetails2()
    {
        ShowCardDetails(card2Details, 2);
    }
    public void CardDetails3()
    {
        ShowCardDetails(card3Details, 3);
    }
    public void CardDetails4()
    {
        ShowCardDetails(card4Details, 4);
    }
    public void CardDetails5()
    {
        ShowCardDetails(card5Details, 5);
    }

    private void ShowCardDetails(CanvasGroup cardDetails, int num)
    {
        PCard pCard = shopManager.GetCardFromPack(num);

        if (pCard.mentor != null)
        {
            cardDetails.GetComponentInChildren<TMP_Text>().text = pCard.mentor.description;
        }
        else if (pCard.textbook != null)
        {
            cardDetails.GetComponentInChildren<TMP_Text>().text = pCard.textbook.description;
        }
        else if (pCard.cardBuff != null)
        {
            cardDetails.GetComponentInChildren<TMP_Text>().text = pCard.cardBuff.name.ToString();
        }
        else
        {
            return;
        }
        cardDetails.blocksRaycasts = true;
        StartCoroutine(FadeIn(cardDetails));
        cardDetails.interactable = true;
    }
    public void RemoveCardDetails1()
    {
        RemoveCardDetails(card1Details);
    }
        public void RemoveCardDetails2()
    {
        RemoveCardDetails(card2Details);
    }
        public void RemoveCardDetails3()
    {
        RemoveCardDetails(card3Details);
    }
        public void RemoveCardDetails4()
    {
        RemoveCardDetails(card4Details);
    }
        public void RemoveCardDetails5()
    {
        RemoveCardDetails(card5Details);
    }

        private void RemoveCardDetails(CanvasGroup cardDetails)
    {
        cardDetails.blocksRaycasts = false;
        StartCoroutine(FadeOut(cardDetails));
        cardDetails.interactable = false;
    }


    private IEnumerator FadeIn(CanvasGroup fadeInObject)//Fade the scene when the quit button is clicked
    {
        float timeToFade = .2f;
        float timeElapsed = 0;
        while (fadeInObject.alpha < 1)
        {
            //float opacity = fadeInObject.alpha - .05f;
            //Mathf.Clamp(opacity, 0, 1);
            fadeInObject.alpha = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            //fadeInObject.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 1;
    }
    private IEnumerator FadeOut(CanvasGroup fadeInObject)//Fade the scene when the quit button is clicked
    {
        float timeToFade = .2f;
        float timeElapsed = 0;
        while (fadeInObject.alpha > 0)
        {
            //float opacity = fadeInObject.alpha - .05f;
            //Mathf.Clamp(opacity, 0, 1);
            fadeInObject.alpha = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            //fadeInObject.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 0;
    }

}
