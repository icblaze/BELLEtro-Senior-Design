using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


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
        // roundSource.clip = RoundMusic;
        // shopSource.clip = ShopMusic;
        // bossSource.clip = BossMusic;
        // defeatSource.clip = DefeatMusic;

        // //Play all music sources to have them synced. Used for transitioning between tracks
        mainMenuSource.Play();
        // roundSource.Play();
        // shopSource.Play();
        // bossSource.Play();
        // defeatSource.Play();

    }
    //Function that plays a given sound effect when called.
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    //Function call to change between music tracks
    public void changeMusic(int curCount)
    {
     
        StartCoroutine(blendTrack(mainMenuSource));

    }

    private IEnumerator blendTrack(AudioSource musicTrack)
    {
        float timeToFade = 2.25f;
        float timeElapsed = 0;
        while (timeElapsed < timeToFade)
        {
            musicTrack.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}