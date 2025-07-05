using System;
using TMPro;
using UnityEngine;

public class DefeatManager : MonoBehaviour
{
    public GameObject defeatText;
    public GameObject backgroundImage;
    private static DefeatManager instance;  //DefeatManager instance varaiable

    //Singleton for the DefeatManager
    public static DefeatManager access()
    {
        if (instance == null)
        {
            instance = new DefeatManager();
        }

        return instance;
    }

    public void SetText()
    {
        //backgroundImage.GetComponent<ScrollingImage>().enabled = true;
        Game inst = Game.access();
        Player player = Player.access();
        String score = "Score of Last Round:" + player.chipCount + "\n";
        String rounds = "Reached Ante: " + inst.anteValue + " Round: " + inst.roundValueTest + "\n";
        String defeated;
        if (inst.roundValueTest == 1)
        {
            defeated = "Defeated By: Small Blind\n";
        }
        else if (inst.roundValueTest == 2)
        {
            defeated = "Defeated By: Big Blind\n";
        }
        else
        {
            defeated = "Defeated By: " + inst.currentSpecialBlind.ToString() + "\n";
        }

        defeatText.GetComponent<TMP_Text>().text = score + rounds + defeated;
        Round round = Round.access();
        round.RestartGame();
    }
}
