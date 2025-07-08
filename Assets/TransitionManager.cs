//Script is used a central place to transition between the different 
//screens. This allows other members from being able to transition
//to different scenes without worrying about its functionality.
//Current Devs: 
//Fredrick Bouloute (bouloutef04)

using UnityEngine;

public class TransitionManager : MonoBehaviour
{
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
    private HorizontalCardHolder horizontalCardHolder;
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
        if (horizontalCardHolder == null)
        {
            horizontalCardHolder = FindFirstObjectByType<HorizontalCardHolder>();
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        StartCoroutine(fadeScript.FadeOut(roundSelectScreen));
        StartCoroutine(fadeScript.FadeIn(roundScreen));
        if (inst.roundValueTest == 3)//If boss/special blind, set boss music
        {
            audioManager.ChangeToBossMusic();
        }

        //  Run Blind Mentor Buffer
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Blind);

        //  Hand Draw After Blind Selected
        StartCoroutine(horizontalCardHolder.OnBlindStart());
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
