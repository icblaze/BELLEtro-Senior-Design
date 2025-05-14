using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Threading.Tasks;
public class MainMenuManager : MonoBehaviour
{
    //This script is used as a manager object for the main menu. This includes function calls
    //for the buttons, audio and transitions.
    private Image transitionImage;
    private Button playButton;
    private Button optionsButton;
    private Button quitButton;
    private AudioManager audioManager;

    //Function called when object is first created/started. Used to obtain objects. Possibly do not need these
    //as buttons should be able to call functions themselves.
    private void Awake()
    {
        transitionImage = GameObject.FindGameObjectWithTag("TransitionImage").GetComponent<Image>();
        // playButton = GameObject.FindGameObjectWithTag("PlayButton").GetComponent<Button>();
        // optionsButton = GameObject.FindGameObjectWithTag("OptionButton").GetComponent<Button>();
        // quitButton = GameObject.FindGameObjectWithTag("QuitButton").GetComponent<Button>();
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
        //audioManager.PlaySFX(audioManager.);
        StartCoroutine(DelayedTransition());
        Debug.Log("StartButton Was Clicked");
    }

    //Function used to bring up options menu. When options button is clicked, it will determine
    //if the menu is opened. If so, it will disable it. Otherwise, it will enable the menu.
    public void OptionClick()
    {
        Debug.Log("Options Button Clicked");

    }

    //Function used to quit application. When the quit button is clicked, application quit runs.
    public void QuitClick()
    {
        Debug.Log("Quit Button Clicked");
        Application.Quit();
    }
    //Function that is called once the animation is done to load the playable scene.
    public void DelayedLoad()
    {
        SceneManager.LoadScene("Balatro-Feel");
    }
}

