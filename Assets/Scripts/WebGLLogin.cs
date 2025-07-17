using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System.Net;
using UnityEngine.SceneManagement;

public class WebGLLogin : MonoBehaviour
{
    private bool loginSuccessful = false;

    void Update()
    {
        if (loginSuccessful == true)
        {
            // Load main menu scene 
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Automatic login that's called by ELLE React website
    public void WebGLLoginAttempt(string loginToken)
    {
        if (loginToken != "")
        {
            BackendHook.StartHookWithCallback(BackendHook.Login(loginToken), OnLoginComplete);
        }
        else
        {
            Debug.LogError("WebGL game attempted to login, but no login token was detected!");
        }
    }

    // Triggers after the above BackendHook.Login() Coroutine finishes
    public void OnLoginComplete()
    {
        if (BackendHook.loginTokenString.Contains("User Not Found") || BackendHook.loginTokenString.Contains("Incorrect"))
        {
            Debug.Log("Login Failed");
            Debug.LogWarning(BackendHook.loginTokenString);
            return;
        }
        Debug.Log("Successful Login: Moving Scenes");
        loginSuccessful = true;
    }
}
