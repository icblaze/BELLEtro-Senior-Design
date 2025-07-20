using UnityEngine;

public class MidtermMaster : Tag
{
    public MidtermMaster() : base(TagNames.MidtermMaster)
    {
        description = "Earn an extra $25 after defeating the special blind";
        tagName = "Midterm Master";
    }

    public override void applyTag()
    {
        GameObject.Find("GameObject").GetComponent<EndOfRoundManager>().extraBossReward = true; 
    }
}
