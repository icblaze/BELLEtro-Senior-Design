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
        String score = "Score of Last Round:" + player.chipCount + "\n\n";
        String rounds = "Reached Ante: " + inst.GetAnte() + " Round: " + inst.GetRound() + "\n\n";
        String defeated;
        if (inst.GetRound() == 1)
        {
            defeated = "Defeated By: Small Blind\n <sprite name=\"SmallBlind\">\n";
        }
        else if (inst.GetRound() == 2)
        {
            defeated = "Defeated By: Big Blind\n <sprite name=\"BigBlind\">\n";
        }
        else
        {
            defeated = "Defeated By: " + inst.currentSpecialBlind.ToString() + "\n <sprite name=\"" + inst.currentSpecialBlind.blindType.ToString() + "\">\n";
        }
        defeatText = GameObject.Find("Defeat Text");
        defeatText.GetComponent<TMP_Text>().text = score + rounds + defeated;

        //  End play session
        BackendHook.StartHook(BackendHook.endSession(inst.GetAnte(), inst.getMostFrequentHandPlayed()));

        Game.access().ResetGame();
        Round.access().RestartGame();
        Player.access().Reset();
        Deck.access().Reset();
    }
}
