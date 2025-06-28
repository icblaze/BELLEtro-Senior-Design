using UnityEngine;
using System.Collections;

//Script is used to create a screen shake effect when called. 
//This can be used in instances such as scoring, deleting cards, rerolls,
//etc.
//Current Devs: Fredrick Bouloute (bouloutef04)

public class ShakeScreen : MonoBehaviour
{

    public AnimationCurve animationCurve;
    public float duration = 0.75f;
    public bool shake;
    private static ShakeScreen instance;
    //Singleton for the screen shake
    public static ShakeScreen access()
    {
        if (instance == null)
        {
            instance = new ShakeScreen();
        }

        return instance;
    }
    public void Update()
    {
        if (shake)
        {
            shake = false;
            StartCoroutine(Shaking());
        }
    }
    public void StartShake()
    {
        StartCoroutine(Shaking());
    }
    IEnumerator Shaking()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            transform.position = startPosition + Random.insideUnitSphere;
            yield return null;
        }
        transform.position = startPosition;
    }
}
