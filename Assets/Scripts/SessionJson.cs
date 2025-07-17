// This script is used to format the session information for communication to the server (save the date and time)
// Developers:
// Robert Morris (momomonkeyman)
// Original VirtuELLE Mentor team

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SessionJson 
{
    public string sessionID;

    public static SessionJson createFromJson(string jsonString)
    {
        return JsonUtility.FromJson<SessionJson>(jsonString);
    }

    public string getSessionID()
    {
        return sessionID;
    }

}
