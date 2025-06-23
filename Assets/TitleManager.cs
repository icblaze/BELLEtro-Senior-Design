using UnityEngine;

//Script is used to manage the title section of the regular UI. This includes
//when in the blind select screen, the shop, and the round screen.
//Current Devs:
//Fredrick Bouloute (bouloutef04)
public class TitleManager : MonoBehaviour
{
    public CanvasGroup selectBlindScreen;
    public CanvasGroup shopScreen;
    public CanvasGroup roundScreen;
    FadeScript fade = FadeScript.access();

    public void changeToBlindScreen()
    {
        StartCoroutine(fade.FadeOut(shopScreen));
        StartCoroutine(fade.FadeOut(roundScreen));
        StartCoroutine(fade.FadeIn(selectBlindScreen));
    }
    public void changeToShopScreen()
    {
        StartCoroutine(fade.FadeOut(selectBlindScreen));
        StartCoroutine(fade.FadeOut(roundScreen));
        StartCoroutine(fade.FadeIn(shopScreen));
    }
    public void changeToRoundScreen()
    {
        StartCoroutine(fade.FadeOut(shopScreen));
        StartCoroutine(fade.FadeOut(selectBlindScreen));
        StartCoroutine(fade.FadeIn(roundScreen));
    }
}
