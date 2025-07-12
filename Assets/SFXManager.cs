//Script is used to ge the different soudn effects and create separate
//functions for each sound as needed. This allows other scripts andor
//objects to call them.
//Current Dev: Fredrick Bouloute (bouloutef04)
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static AudioManager audioManager;
    private static AudioClip cardScoring;
    private static AudioClip multScoring;
    private static AudioClip buttonPress;
    private static AudioClip moneyUsed;
    private static AudioClip no;//Simply for instances where something can't be done such as selected an unselectable card
    private static AudioClip roundWon;
    private static AudioClip gameWon;
    private static AudioClip gameLost;
    private static AudioClip cardSelected;
    private static SFXManager sfxManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        buttonPress = Resources.Load<AudioClip>($"SFX/Button");
        cardScoring = Resources.Load<AudioClip>($"SFX/Hollow");
        multScoring = Resources.Load<AudioClip>($"SFX/Whoosh");
        moneyUsed = Resources.Load<AudioClip>($"SFX/Metal Dropping");
        roundWon = Resources.Load<AudioClip>($"SFX/Level Completion");
        no = Resources.Load<AudioClip>($"SFX/Stingz");
        gameWon = Resources.Load<AudioClip>($"SFX/GameWon");
        gameLost = Resources.Load<AudioClip>($"SFX/GameLost");
        cardSelected = Resources.Load<AudioClip>($"SFX/CardSelected");
    }
    public void ButtonSFX()
    {
        audioManager.PlaySFX(buttonPress);
    }
    public void PurchasedMade()
    {
        audioManager.PlaySFX(moneyUsed);
    }
    public void CardScoreSFX()
    {
        audioManager.PlaySFX(cardScoring);
    }
    public void MultScoreSFX()
    {
        audioManager.PlaySFX(multScoring);
    }
    public void NoSFX()
    {
        audioManager.PlaySFX(no);
    }
    public void MoneyUsed()
    {
        audioManager.PlaySFX(moneyUsed);
    }
    public void RoundWonSFX()
    {
        audioManager.PlaySFX(roundWon);
    }
    public void GameWonSFX()
    {
        audioManager.PlaySFX(gameWon);
    }
    public void GameLostSFX()
    {
        audioManager.PlaySFX(gameLost);
    }
    public void CardSelected()
    {
        audioManager.PlaySFX(cardSelected);
    }
    public void VoiceSFX(AudioClip voice)
    {
        audioManager.PlayVoice(voice);
    }
}
