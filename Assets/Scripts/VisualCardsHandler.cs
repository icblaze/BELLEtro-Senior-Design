// This Document contains the code for instantiating a GameObject
// that parents all the card visuals, needed to manage hierarchy
// Current Devs:
// Van: created the class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualCardsHandler : MonoBehaviour
{

    public static VisualCardsHandler instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
