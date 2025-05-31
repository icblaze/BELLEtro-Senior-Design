using UnityEngine;
//Script is used to close out the RunInfo Manager.
public class RunInfoManager : MonoBehaviour
{
    public CanvasGroup runInfo;

    //Function called to go back to regular gameplay. Make run info menu
    //disappear
    public void BackButton()
    {
        runInfo.alpha = 0;
        runInfo.blocksRaycasts = false;
    }

}
