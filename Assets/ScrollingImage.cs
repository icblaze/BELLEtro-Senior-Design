//Script is used ot create a scrolling effect on 
//background images. 
//Current Devs:
//Fredrick Bouloute (bouloutef04)

using UnityEngine;
using UnityEngine.UI;

public class ScrollingImage : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private float x, y;
    [SerializeField] private float rotation;
    private float z = 0;
    public GameObject imageObj;
    // Update is called once per frame
    void Update()
    {
        z += rotation;
        image.uvRect = new Rect(image.uvRect.position + new Vector2(x, y) * Time.deltaTime, image.uvRect.size);
        imageObj.GetComponent<RectTransform>().eulerAngles = new Vector3(0,0, z);
    }
}
