using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script is used to manage the audio including music, sfx functions,
//transitions, etc.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioSource mainMenuSource;
    [SerializeField] AudioSource roundSource;
    [SerializeField] AudioSource shopSource;
    [SerializeField] AudioSource bossSource;
    [SerializeField] AudioSource defeatSource;
    [SerializeField] AudioSource sfxSource;
    [Header("-------Audio Clips----------")]
    public AudioClip MainMenuMusic;
    public AudioClip RoundMusic;
    public AudioClip ShopMusic;
    public AudioClip BossMusic;
    public AudioClip DefeatMusic;
    private GameObject sceneCounter;


    //Initial FUnction call to set variables
    private void Start()
    {
        // //sceneCounter = GameObject.Find("Scene Counter");
        // //Apply each audio clip to its source
        mainMenuSource.clip = MainMenuMusic;

        roundSource.clip = RoundMusic;
        roundSource.volume = 0;

        shopSource.clip = ShopMusic;
        shopSource.volume = 0;

        bossSource.clip = BossMusic;
        bossSource.volume = 0;

        defeatSource.clip = DefeatMusic;
        defeatSource.volume = 0;

        // //Play all music sources to have them synced. Used for transitioning between tracks
        mainMenuSource.Play();
        roundSource.Play();
        shopSource.Play();
        bossSource.Play();
        defeatSource.Play();

    }
    //Function that plays a given sound effect when called.
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    //Function call to change between music tracks
    private void changeMusic(AudioSource originalTrack, AudioSource newTrack)
    {
        StartCoroutine(blendTrack(originalTrack, newTrack));
    }

    public void ChangeToMainMenuMusic()
    {
        if (roundSource.volume >= .9f)
        {
            changeMusic(roundSource, mainMenuSource);
        }
        else if (shopSource.volume >= .9f)
        {
            changeMusic(shopSource, mainMenuSource);
        }
        else if (bossSource.volume >= .9f){
            changeMusic(bossSource, mainMenuSource);
        }
        else {
            changeMusic(defeatSource, mainMenuSource);
        }
    }

    public void ChangeToRoundMusic()
    {
        if (mainMenuSource.volume >= .9f)
        {
            changeMusic(mainMenuSource, roundSource);
        }
        else
        {
            //Anytime leaving shop, player will go to round music.
            changeMusic(shopSource, roundSource);
        }
    }
    public void ChangeToBossMusic()
    {
        //Round music will always play before going into boss
        changeMusic(roundSource, bossSource);
    }
    public void ChangeToShopMusic()
    {
        //If player finished round, go to shop
        if (roundSource.volume >= .9f)
        {
            changeMusic(roundSource, shopSource);
        }
        else//If player finished boss round
        {
            changeMusic(bossSource, shopSource);
        }
    }
    public void ChangeToDeathMusic()
    {
        //If player lost in regular round
        if (roundSource.volume >= .9f)
        {
            changeMusic(roundSource, defeatSource);
        }
        else//If player lost in boss round
        {
            changeMusic(bossSource, defeatSource);
        }
    }

    private IEnumerator blendTrack(AudioSource musicTrack, AudioSource newTrack)
    {
        float timeToFade = 2.25f;
        float timeElapsed = 0;
        while (timeElapsed < timeToFade)//Fade out musicTrack and fade in newTrack
        {
            musicTrack.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
            newTrack.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}