//  This class changes the selected shader used for each Card object
//  Not only does this work with PCard's, but also work with Mentors
//  Andy (flakkid): created class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShaderCodePCard : MonoBehaviour
{
    [SerializeField] private GameObject baseLayer;              //Base layer of the card
    [SerializeField] private GameObject suitLayer;              //Suit layer of the card
    [SerializeField] private GameObject enhancementLayer;       //Enhancement layer of the card 

    Image base_img;
    Image suit_img;
    Image enhancement_img;

    Material base_m;
    Material suit_m;
    Material enhancement_m;

    PCardVisual visual;
    string[] editions = { "REGULAR", "POLYCHROME", "FOIL", "NEGATIVE"};

    void Awake()
    {
        base_img = baseLayer.GetComponent<Image>();
        base_m = new Material(base_img.material);
        base_img.material = base_m;

        suit_img = suitLayer.GetComponent<Image>();
        suit_m = new Material(suit_img.material);
        suit_img.material = suit_m;

        enhancement_img = enhancementLayer.GetComponent<Image>();
        enhancement_m = new Material(enhancement_img.material);
        enhancement_img.material = enhancement_m;
    }

    // Update is called once per frame
    void Update()
    {

        // Get the current rotation as a quaternion
        Quaternion currentRotation = transform.parent.localRotation;

        // Convert the quaternion to Euler angles
        Vector3 eulerAngles = currentRotation.eulerAngles;

        // Get the X-axis angle
        float xAngle = eulerAngles.x;
        float yAngle = eulerAngles.y;

        // Ensure the X-axis angle stays within the range of -90 to 90 degrees
        xAngle = ClampAngle(xAngle, -90f, 90f);
        yAngle = ClampAngle(yAngle, -90f, 90);


        base_m.SetVector("_Rotation", new Vector2(ExtensionMethods.Remap(xAngle,-20,20,-.5f,.5f), ExtensionMethods.Remap(yAngle, -20, 20, -.5f, .5f)));
        suit_m.SetVector("_Rotation", new Vector2(ExtensionMethods.Remap(xAngle, -20, 20, -.5f, .5f), ExtensionMethods.Remap(yAngle, -20, 20, -.5f, .5f)));
        enhancement_m.SetVector("_Rotation", new Vector2(ExtensionMethods.Remap(xAngle, -20, 20, -.5f, .5f), ExtensionMethods.Remap(yAngle, -20, 20, -.5f, .5f)));

    }

    // Method to clamp an angle between a minimum and maximum value
    float ClampAngle(float angle, float min, float max)
    {
        if (angle < -180f)
            angle += 360f;
        if (angle > 180f)
            angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }

    //  When Edition of PCard is updated, change shaders
    public void UpdateEdition(CardEdition edition)
    {
        for (int i = 0; i < base_img.material.enabledKeywords.Length; i++)
        {
            base_img.material.DisableKeyword(base_img.material.enabledKeywords[i]);
            suit_img.material.DisableKeyword(suit_img.material.enabledKeywords[i]);
            enhancement_img.material.DisableKeyword(enhancement_img.material.enabledKeywords[i]);
        }

        string chosenEdition = editions[0];
        switch (edition)
        {
            case CardEdition.Polychrome:
                chosenEdition = editions[1];
                break;
            case CardEdition.Foil:
                chosenEdition = editions[2];
                break;
            case CardEdition.Negative:
                chosenEdition = editions[3];
                break;
        }

        base_img.material.EnableKeyword("_EDITION_" + chosenEdition);
        suit_img.material.EnableKeyword("_EDITION_" + chosenEdition);
        enhancement_img.material.EnableKeyword("_EDITION_" + chosenEdition);
    }
}
