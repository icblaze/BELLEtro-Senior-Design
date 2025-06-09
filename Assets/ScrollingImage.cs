using UnityEngine;
using UnityEngine.UI;

//Script is used ot create a scrolling effect on 
//background images. 
//Current Devs:
//Fredrick Bouloute (bouloutef04)

public class ScrollingImage : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float x, y;
    // Update is called once per frame
    void Update()
    {
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime, image.uvRect.size);
    }
}
