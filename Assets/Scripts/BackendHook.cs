// This script connects the Website/database with the game so we can check login and record time playing 
// Developers: 
// Robert Morris (momomonkeyman)
// Developers of VirtuELLE Mentor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System.Linq;
using Newtonsoft.Json;



public class BackendHook : MonoBehaviour
{
    public const string BASE_URL = "https://chdr.cs.ucf.edu/elle";
    public const string API_ENDPOINT = "https://chdr.cs.ucf.edu/elleapi";
    public static string loginToken;
    public static int userID;
    public static string loginTokenString;
    public static string sessionID;
 
    // This is the script for manual login into ELLE Servers
    public static IEnumerator Login(string username, string password)
    {
        string url = API_ENDPOINT + "/login";

        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("password", password);
        UnityWebRequest loginRequest = UnityWebRequest.Post(url, form);

        yield return loginRequest.SendWebRequest();
        

        if (loginRequest.result == UnityWebRequest.Result.ConnectionError || loginRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(loginRequest.error);
        }
        else
        {
            // Debug.Log("Upload complete!");
        }

        loginTokenString = loginRequest.downloadHandler.text;
        TokenJson t = TokenJson.createFromJson(loginTokenString);

        // Record login token and user ID for future use
        loginToken = t.access_token;
        userID = t.id;
        Debug.Log("USER ID: " + userID);
        loginRequest.disposeUploadHandlerOnDispose = true;
        loginRequest.disposeDownloadHandlerOnDispose = true;
        loginRequest.Dispose();
    }
    
    // Automatic login used when built using WebGL and hosted on website
    public static IEnumerator Login(string token)
    {
        WebGLLoginData loginItems = JsonConvert.DeserializeObject<WebGLLoginData>(token);
        // Record login token for future use
        loginToken = loginItems.jwt;
        userID = loginItems.userID;
        loginTokenString = "";
        yield return loginToken;
    }

    // Starts the recording of the time the game is played for and the info about the game into the server (like what module it is)
    public static IEnumerator startSession()
    {
        string url = API_ENDPOINT + "/session";
        WWWForm form = new WWWForm();

        string sessionDate = DateTime.Now.ToString(@"MM\/dd\/yy");
        string startTime = DateTime.Now.ToString("HH:mm");

        form.AddField("moduleID", 66);
        form.AddField("sessionDate", sessionDate);
        form.AddField("startTime", startTime);
        form.AddField("platform", "cp");

        UnityWebRequest startSessionRequest = UnityWebRequest.Post(url, form);

        startSessionRequest.SetRequestHeader("Authorization", "Bearer " + loginToken);

        // Debug.Log("Test Start " + startTime);

        yield return startSessionRequest.SendWebRequest();

        string sessionJsonString = startSessionRequest.downloadHandler.text;

        SessionJson session = SessionJson.createFromJson(sessionJsonString);

        sessionID = session.getSessionID();

        startSessionRequest.disposeUploadHandlerOnDispose = true;
        startSessionRequest.disposeDownloadHandlerOnDispose = true;
        startSessionRequest.Dispose();
    }
    
    // This function records the final time of the session and the player's points
    public static IEnumerator endSession()
    {
        string url = API_ENDPOINT + "/endsession";
        WWWForm form = new WWWForm();

        DateTime endTime = DateTime.Now;
        
        // Due to the time still progressing while the player is paused, we keep track of how long the player is paused and subtract it from the time the player ended the session.
        // UPDATE: The BELLEtro team has decided that our game is inherently "AFKable" as there is no round time limit so we are removing This
        // To reinstate this function of the game, you must rename the previous line's endTime to timeNow and then add a PauseScript to keep track of total paused time
        //DateTime endTime = timeNow.AddSeconds(-1 * PauseScript.pauseTimer);

        string endTimeString = endTime.ToString("HH:mm");

        Debug.Log(endTime + " | " + endTimeString);

        // Debug.Log("end time modified: " + endTimeString);
        // Debug.Log("end time original: " + endTimeStringBefore);

        form.AddField("sessionID", sessionID);
        form.AddField("endTime", endTimeString);
        form.AddField("playerScore", Game.ante.ToString());

        UnityWebRequest endSessionRequest = UnityWebRequest.Post(url, form);

        endSessionRequest.SetRequestHeader("Authorization", "Bearer " + loginToken);

        Time.timeScale = 0;
        
        yield return endSessionRequest.SendWebRequest();

        Time.timeScale = 1;

        if (endSessionRequest.result == UnityWebRequest.Result.ConnectionError || endSessionRequest.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(endSessionRequest.error);
        }
        else
        {
            // Debug.Log("endSession Upload complete!");
        }

        endSessionRequest.disposeUploadHandlerOnDispose = true;
        endSessionRequest.disposeDownloadHandlerOnDispose = true;
        endSessionRequest.Dispose();
    }
}

