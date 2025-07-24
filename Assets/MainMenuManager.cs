//This script is used as a manager object for the main menu. This includes function calls
//for the buttons, audio and transitions.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class MainMenuManager : MonoBehaviour
{

    private Image transitionImage;
    private Button playButton;
    private Button optionsButton;
    private AudioManager audioManager;
    public CanvasGroup fadeToBlack;
    public CanvasGroup optionsMenu;
    public CanvasGroup howToPlayMenu;
    public CanvasGroup collections;
    public GameObject colletionsObject;
    public GameObject particles;
    public TMP_Text text;

    //Function called when object is first created/started. Used to obtain objects. Possibly do not need these
    //as buttons should be able to call functions themselves.
    private void Awake()
    {
        transitionImage = GameObject.FindGameObjectWithTag("TransitionImage").GetComponent<Image>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        Debug.Log("Main Menu Manager has Awoken");

    }

    //Function for scene transitions. While the screen in not black, decrease 
    //aplha/opacity until the screen is dark. Then call the load function to transition.
    public IEnumerator DelayedTransition()
    {
        float elapsedPercentage = 0;
        float elapsedTime = 0;
        float duration = .5f;
        //Get colors for transitions
        Color startColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 0);
        Color targetColor = new Color(transitionImage.color.r, transitionImage.color.g, transitionImage.color.b, 1);

        //While alpha is not max, increase alpha
        while (elapsedPercentage < 1)
        {
            elapsedPercentage = elapsedTime / duration;
            transitionImage.color = Color.Lerp(startColor, targetColor, elapsedPercentage);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        Invoke("DelayedLoad", .5f);
    }

    //Function called when start button is clicked. Plays a sound effect and starts a delayed 
    //function call to transition to the playable space
    public void StartClick()
    {
        StartCoroutine(FadeToBlack());
        Debug.Log("StartButton Was Clicked");

        //  Start play session
        BackendHook.StartHook(BackendHook.startSession());
    }

    //Function used to bring up options menu. When options button is clicked, it will determine
    //if the menu is opened. If so, it will disable it. Otherwise, it will enable the menu.
    public void OptionClick()
    {
        Debug.Log("Options Button Clicked");
        //If menu is not present, display. 
        if (optionsMenu.alpha == 0)
        {
            optionsMenu.alpha = 1;
            optionsMenu.blocksRaycasts = true;
        }//else, close menu. This is a non-issue as player isn't able to hit button when menu
        else
        {//is open but this is just to ensure it works
            optionsMenu.alpha = 0;
            optionsMenu.blocksRaycasts = false;
        }
    }

    //Function used to quit application. When the quit button is clicked, application quit runs.
    public void QuitClick()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
    //Function is used to bring up the howtoplay menu in main menu.
    public void HowToPlayClick()
    {
        howToPlayMenu.alpha = 1;
        howToPlayMenu.blocksRaycasts = true;
        particles.SetActive(false);
    }

    //Function that is called once the animation is done to load the playable scene.
    public void DelayedLoad()
    {
        SceneManager.LoadScene("Balatro-Feel 1");
        audioManager.ChangeToRoundMusic();
    }

    private IEnumerator FadeToBlack()//Fade the scene when the quit button is clicked
    {
        while (fadeToBlack.alpha < 1)
        {
            float opacity = fadeToBlack.alpha + .01f;
            Mathf.Clamp(opacity, 0, 1);
            fadeToBlack.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        Invoke("DelayedLoad", .5f);
    }

    //Function is used to change "pages" in the HowToPlay Menu. 
    //In this case, it is the next page.
    public void NextPage()
    {
        if (text.pageToDisplay == 25)
        {
            //Nothing
        }
        else
        {
            text.pageToDisplay++;
        }
    }
    //Function is used to change "pages" in the HowToPlay Menu. 
    //In this case, it is the back page.
    public void BackPage()
    {
        if (text.pageToDisplay == 1)
        {
            //Nothing
        }
        else
        {
            text.pageToDisplay--;
        }
    }

    public void BackButtonHowToPlay()
    {
        howToPlayMenu.alpha = 0;
        howToPlayMenu.blocksRaycasts = false;
        particles.SetActive(true);
    }
    public void CollectionsButton()
    {
        particles.SetActive(false);
        collections.alpha = 1;
        collections.blocksRaycasts = true;
        colletionsObject.SetActive(true);
    }
    public void CollectionsBackButton()
    {
        particles.SetActive(true);
        collections.alpha = 0;
        collections.blocksRaycasts = false;
        colletionsObject.SetActive(false);
    }
}

