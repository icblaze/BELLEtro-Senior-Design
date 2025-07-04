using UnityEngine;
//Script is used a central place to transition between the different 
//screens. This allows other members from being able to transition
//to different scenes without worrying about its functionality.
//Current Devs: 
//Fredrick Bouloute (bouloutef04)
public class TransitionManager : MonoBehaviour
{
    private static TransitionManager instance;  //TransitionManager instance varaiable

    //Singleton for the TransitionManager
    public static TransitionManager access()
    {
        if (instance == null)
        {
            instance = new TransitionManager();
        }

        return instance;
    }

    public CanvasGroup roundSelectScreen;

    public CanvasGroup roundScreen;

    public CanvasGroup endOfRoundScreen;
    public CanvasGroup shopScreen;
    public CanvasGroup packScreen;
    public CanvasGroup defeatScreen;
    public CanvasGroup regularUI;
    private FadeScript fadeScript = FadeScript.access();
    private AudioManager audioManager;
    private ShopManager shopManager;
    private Game inst = Game.access();

    public void TransitionToRoundSelect()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        //If player opens packs from skip tag
        if (packScreen.alpha >= .9f)
        {
            StartCoroutine(fadeScript.FadeOut(packScreen));
            StartCoroutine(fadeScript.FadeIn(roundSelectScreen));
        }
        else//Transition from round shop to round select
        {
            StartCoroutine(fadeScript.FadeOut(shopScreen));
            StartCoroutine(fadeScript.FadeIn(roundSelectScreen));
            audioManager.ChangeToRoundMusic();
        }
    }
    public void TransitionToRoundScreen()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StartCoroutine(fadeScript.FadeOut(roundSelectScreen));
        StartCoroutine(fadeScript.FadeIn(roundScreen));
        if (inst.roundValue == 3)//If boss/special blind, set boss music
        {
            audioManager.ChangeToBossMusic();
        }
    }
    public void TransitionToEndOfRoundScreen()
    {
        StartCoroutine(fadeScript.FadeOut(roundScreen));
        StartCoroutine(fadeScript.FadeIn(endOfRoundScreen));
        EndOfRoundManager endOfRoundManager = FindFirstObjectByType<EndOfRoundManager>();
        endOfRoundManager.EndScreenOpened();
    }
    public void TransitionToShopScreen()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StartCoroutine(fadeScript.FadeOut(endOfRoundScreen));
        StartCoroutine(fadeScript.FadeIn(shopScreen));
        audioManager.ChangeToShopMusic();
        Debug.Log("Transitioning");
    }

    public void TransitionToDefeatScreen()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StartCoroutine(fadeScript.FadeOut(regularUI));
        StartCoroutine(fadeScript.FadeOut(roundScreen));
        StartCoroutine(fadeScript.FadeIn(defeatScreen));
        audioManager.ChangeToDeathMusic();
        DefeatManager defeatManager = DefeatManager.access();
        defeatManager.SetText();
    }
}
