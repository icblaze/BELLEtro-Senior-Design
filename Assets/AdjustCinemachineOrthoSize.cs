//  This Document contains the code for changing the orthographic size
//  based on the aspect ratio.

// Current Devs:
// Andy (flakkid): Made initial script 

using UnityEngine;
using Cinemachine;

public class AdjustCinemachineOrthoSize : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public float targetAspect = 16f / 9f;
    public float baseOrthoSize = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        float currentAspect = (float)Screen.width / Screen.height;
        float scale = targetAspect / currentAspect;

        if (vCam != null)
        {
            vCam.m_Lens.OrthographicSize = baseOrthoSize * Mathf.Max(1f, scale);
        }
    }
}