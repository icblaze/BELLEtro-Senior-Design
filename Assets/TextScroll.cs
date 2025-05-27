using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class TextScroll : MonoBehaviour
{
    public TMP_Text text;
    public void Start(){
        
    }
    // Start is called before the first frame update
    public void NextPage(){
        if(text.pageToDisplay == 4){
            //Nothing
        }
        else{
            text.pageToDisplay++;
        }
    }
    public void BackPage(){
        if(text.pageToDisplay == 1){
            //Nothing
        }
        else{
            text.pageToDisplay--;
        }
    }
}
