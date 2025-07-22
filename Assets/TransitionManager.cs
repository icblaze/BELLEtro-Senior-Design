//Script is used a central place to transition between the different 
//screens. This allows other members from being able to transition
//to different scenes without worrying about its functionality.
//Current Devs: //
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
    public CanvasGroup victoryScreen;
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
            TitleManager titleManager = GameObject.FindFirstObjectByType<TitleManager>().GetComponent<TitleManager>();
            titleManager.changeToRoundSelectScreen();
            BlindSceneManager blindSceneManager = GameObject.Find("BlindSceneManager").GetComponent<BlindSceneManager>();
            blindSceneManager.SetBlindScreenInfo();
        }
    }
    public void TransitionToRoundScreen()
    {
        if (horizontalCardHolder == null)
        {
            horizontalCardHolder = FindFirstObjectByType<HorizontalCardHolder>();
        }

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        if (audioManager == null)
            Debug.LogError("No GameObject found with tag 'Audio'");
        StartCoroutine(fadeScript.FadeOut(roundSelectScreen));
        StartCoroutine(fadeScript.FadeIn(roundScreen));
        TitleManager titleManager = GameObject.FindFirstObjectByType<TitleManager>().GetComponent<TitleManager>();
        titleManager.changeToRoundScreen();
        titleManager.setRoundTitle();
        if (inst.GetRound() == 3)//If boss/special blind, set boss music
        {
            audioManager.ChangeToBossMusic();
        }

        //  Reset MaxHandCount/MaxDiscards
        GameObject.FindFirstObjectByType<PlayHand>().ResetHandCount();
        GameObject.FindFirstObjectByType<DeleteCard>().ResetDiscards();

        //  Run Blind Mentor Buffer
        MentorBufferManager.access().AssignToBuffer();
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
        TitleManager titleManager = GameObject.FindFirstObjectByType<TitleManager>().GetComponent<TitleManager>();

        //  After generating new shop, activate Shop Mentors
        ShopManager shopManager = GameObject.FindFirstObjectByType<ShopManager>().GetComponent<ShopManager>();
        shopManager.NewShop();
        shopManager.resetShopMentor();
        MentorBufferManager.access().AssignToBuffer();
        MentorBufferManager.access().RunBufferImmediate(UseLocation.Shop);

        titleManager.changeToShopScreen();
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

    public void TransitionToVictoryScreen()
    {
        SFXManager sfxManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SFXManager>();
        sfxManager.GameWonSFX();
        StartCoroutine(fadeScript.FadeIn(victoryScreen));
        VictoryManager victoryManager = GameObject.Find("VictoryManager").GetComponent<VictoryManager>();
        victoryManager.GameWon();
    }
}
