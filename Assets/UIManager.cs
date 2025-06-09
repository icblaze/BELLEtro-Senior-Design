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
}