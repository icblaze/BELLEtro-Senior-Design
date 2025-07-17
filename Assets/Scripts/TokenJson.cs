// This Script helps generate JsonTokens 
// Developers:
// Robert Morris (momomonkeyman)
// Original VirtuELLE Mentor team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TokenJson 
{
    public string access_token;
    public int id;

    public static TokenJson createFromJson(string jsonString)
    {
        return JsonUtility.FromJson<TokenJson>(jsonString);
    }
}
