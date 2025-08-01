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
            cardDetails.GetComponentInChildren<TMP_Text>().text = "Edition:" + pCard.edition + "\n" + SplitString.SplitCase.Split(pCard.mentor.name.ToString()) + "\n" + pCard.mentor.description.ToString();
        }
        else if (pCard.textbook != null)
        {
            cardDetails.GetComponentInChildren<TMP_Text>().text = "Edition:" +  pCard.edition + "\n" +  pCard.textbook.GetDescription();
        }
        else if (pCard.cardBuff != null)
        {
            cardDetails.GetComponentInChildren<TMP_Text>().text = "Edition:" +  pCard.edition + "\n" +  SplitString.SplitCase.Split(pCard.cardBuff.name.ToString()) + "\n" + pCard.cardBuff.GetDescription();
        }
        else
        {
            cardDetails.GetComponentInChildren<TMP_Text>().text = "Edition:" +  pCard.edition + "\n" +  pCard.term.ToString() + "\n Chips: " + pCard.chips + "Mult: " + pCard.multiplier;
        }
        // cardDetails.blocksRaycasts = true;
        StartCoroutine(FadeIn(cardDetails));
        // cardDetails.interactable = true;
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
            fadeInObject.alpha = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
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
            fadeInObject.alpha = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return new WaitForSecondsRealtime(.01f);
        }
        fadeInObject.alpha = 0;
    }

}
