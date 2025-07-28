//Functions ensures that PlayerPrefs for term visibily is set. If not, it sets them to visible (false). 
//Otherwise, it loads the values.
//Current Devs: Andy Van (flakkid)


using UnityEngine.UI;
using UnityEngine;

public class HideTermManager : MonoBehaviour
{
    [SerializeField] private Toggle termToggle;
    [SerializeField] private int hideIntStatus;

    // Set to visible by default
    void Start()
    {
        if (!PlayerPrefs.HasKey("HIDE_TERM"))
        {
            PlayerPrefs.SetInt("HIDE_TERM", 0);
        }

        Load();
    }

    //  Set toggle to player pref
    private void Load()
    {
        bool toggleStatus = PlayerPrefs.GetInt("HIDE_TERM") == 1 ? true : false;
        termToggle.isOn = toggleStatus;
    }

    //  Save toggle status as player pref
    private void Save()
    {
        int toggleStatus = termToggle.isOn ? 1 : 0;
        hideIntStatus = toggleStatus;
        PlayerPrefs.SetInt("HIDE_TERM", toggleStatus);
        PlayerPrefs.Save();
    }

    //  Set toggle status
    public void SetVisibility()
    {
        int toggleStatus = termToggle.isOn ? 1 : 0;
        hideIntStatus = toggleStatus;
        PlayerPrefs.SetInt("HIDE_TERM", toggleStatus);
        PlayerPrefs.Save();
    }
}
