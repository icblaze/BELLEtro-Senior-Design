//This script is used for creating the functionality of the pause menu.
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class PauseMenuManager : MonoBehaviour
{
    private Button continueButton;
    private Button optionsButton;
    private Button quitButton;
    public CanvasGroup pauseMenu;
    public CanvasGroup optionsMenu;
    public CanvasGroup fadeToBlack;
    //When the play button is clicked, make the pause menu disappear and not block gameplay.
    public void ContinueClick()
    {
        pauseMenu.alpha = 0;
        pauseMenu.blocksRaycasts = false;
        Debug.Log("PlayButton Was Clicked");
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

    //Function used to return to main menu. A fade to black transition should occur and the 
    //the scene should change soon after.
    public void QuitClick()
    {
        //  End play session
        BackendHook.StartHook(BackendHook.endSession(Game.access().GetAnte()));

        Game.access().ResetGame();
        Round.access().RestartGame();
        Player.access().Reset();
        Deck.access().Reset();

        AudioManager audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        audioManager.ChangeToMainMenuMusic();
        StartCoroutine(FadeToBlack());

        Debug.Log("QuitButton Was Clicked");
    }
    //Load the main menu scene. If victory screen reached, deactivate the particle system.
    public void DelayedLoad()
    {
        VictoryManager  victoryManager = GameObject.FindFirstObjectByType<VictoryManager>().GetComponent<VictoryManager>();
        if (victoryManager.victoryParticles.activeSelf == true)
        {
            victoryManager.victoryParticles.SetActive(false);
        }
        SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator FadeToBlack()//Fade the scene when the quit button is clicked
    {
        //While the black image is still transparent, increase it's alpha and wait .01 seconds.
        while (fadeToBlack.alpha < 1)
        {
            float opacity = fadeToBlack.alpha + .01f;
            Mathf.Clamp(opacity, 0, 1);
            fadeToBlack.alpha = opacity;
            yield return new WaitForSecondsRealtime(.01f);
        }
        //Once finished, call delayed load function in half a second.
        Invoke("DelayedLoad", .5f);
    }
}