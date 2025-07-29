using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class AccessibilityManager : MonoBehaviour
{
    public GameObject Trippy_BG;
    public ShakeScreen shakeScreen;
    public CinemachineBrain cinemachineBrain;
    public ScrollingImage scrollingImage;
    public GameObject particles;
    public bool reduced;
    [SerializeField] private Toggle reducedToggle;

    void Start()
    {
        scrollingImage = GameObject.FindFirstObjectByType<ScrollingImage>().GetComponent<ScrollingImage>();
        if (!PlayerPrefs.HasKey("REDUCED_MOTION"))
        {
            PlayerPrefs.SetInt("REDUCED_MOTION", 0);
        }
        Load();
    }

    private void Load()//Load the opposite of what is saved and call the function to correct.
    {
        reduced = PlayerPrefs.GetInt("REDUCED_MOTION") == 1 ? true : false;
        reducedToggle.isOn = reduced;
    }
     private void Save()
    {
        int toggleStatus = reducedToggle.isOn ? 1 : 0;
        PlayerPrefs.SetInt("REDUCED_MOTION", toggleStatus);
        PlayerPrefs.Save();
    }

    public void ReducedMovement()
    {
        if (!reducedToggle.isOn)//If reduced is active, deactivate
        {
            if (Trippy_BG != null)//if in playable scene
            {
                Trippy_BG.SetActive(true);
                shakeScreen.SetNoShake(false);
                cinemachineBrain.enabled = true;
            }
            if (particles != null)//if in mainmenu
            {

                particles.SetActive(true);
            }
            scrollingImage.enabled = true;
        }
        else
        {
            if (Trippy_BG != null)
            {
                Trippy_BG.SetActive(false);
                shakeScreen.SetNoShake(true);
                cinemachineBrain.enabled = false;
            }
            if (particles != null)
            {
                particles.SetActive(false);
            }
            scrollingImage.enabled = false;
        }
        Save();
    }
}
