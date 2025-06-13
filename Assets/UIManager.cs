using TMPro;
using UnityEngine;
//Script is made to control and manage the UI of the game excluding the 
//card playing area and the shop. This includes the side bar and any pop-ups.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
public class UIManager : MonoBehaviour
{
    public CanvasGroup runInfo;
    public CanvasGroup pauseMenu;
    public GameObject moneyText;
    public CanvasGroup optionsMenu;

    public void PauseButton()
    {
        pauseMenu.alpha = 1;
        pauseMenu.blocksRaycasts = true;
    }

    public void RunInfoButton()
    {
        runInfo.alpha = 1;
        runInfo.blocksRaycasts = true;
    }

    //Function called to go back to regular gameplay. Make run info menu
    //disappear
    public void BackRunInfoButton()
    {
        runInfo.alpha = 0;
        runInfo.blocksRaycasts = false;
    }

    public void ChangeMoneyText(int money)
    {
        moneyText.GetComponentInChildren<TMP_Text>().SetText("$" + money);
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
}