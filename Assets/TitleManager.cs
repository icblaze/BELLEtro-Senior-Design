using TMPro;
using UnityEngine;
using UnityEngine.UI;

//Script is used to manage the title section of the regular UI. This includes
//when in the blind select screen, the shop, and the round screen.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
public class TitleManager : MonoBehaviour
{
    //Canvas Groups to transition
    public CanvasGroup selectBlindScreen;
    public CanvasGroup shopScreen;
    public CanvasGroup roundScreen;
    //Text Components for Round Screne
    public GameObject blindName;
    public GameObject scoreRequirement;
    public GameObject rewardAmount;
    //Symbol/Badge for the round
    public GameObject roundSymbol;
    //Accessing Fade Script Functions
    FadeScript fade = FadeScript.access();

    private static TitleManager instance;
    public static TitleManager access()
    {
        if (instance == null)
        {
            instance = new TitleManager();
        }
        return instance;
    }
    public void changeToRoundSelectScreen()//Change from shop to round/blind select
    {
        StartCoroutine(fade.FadeOut(shopScreen));

        StartCoroutine(fade.FadeIn(selectBlindScreen));
    }
    public void changeToShopScreen()//Change from Round Screen to Shop Screen
    {
        StartCoroutine(fade.FadeOut(roundScreen));
        StartCoroutine(fade.FadeIn(shopScreen));
    }
    public void changeToRoundScreen()//Change from Round Select to Round Screen
    {

        setRoundTitle();
        StartCoroutine(fade.FadeOut(selectBlindScreen));
        StartCoroutine(fade.FadeIn(roundScreen));
    }

    public void setRoundTitle()//Sets the titles when loading into a round
    {
        //Set the round title and its components
        scoreRequirement.GetComponent<TMP_Text>().text = "" /* + round.scoreRequirment*/;
        rewardAmount.GetComponent<TMP_Text>().text = "$" /*+ round.reward*/;
        roundSymbol.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Round/" /*+ round.name*/);
    }
}
