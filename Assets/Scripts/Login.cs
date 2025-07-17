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

public class Login : MonoBehaviour
{

    public static bool usernameSelected = false;
    public static bool passwordSelected = false;
    public InputField usernameInputField;
    public InputField passwordInputfield;
    public Text username;
    public Text password;
    public List<InputField> fields;
    private int _fieldIndexer;
    private bool loginPending = false;  // Used to prevent spamming the login API in case the user holds down the Enter key
    private bool loginSuccessful = false;

    void Start()
    {
        fields = new List<InputField> { usernameInputField, passwordInputfield };

        usernameInputField.Select();
        _fieldIndexer = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (fields.Count <= _fieldIndexer)
            {
                _fieldIndexer = 0;
            }
            fields[_fieldIndexer].Select();
            _fieldIndexer++;
        }

        if (Input.GetKeyDown(KeyCode.Return) && !loginPending)
        {
            LoginAttempt();
        }

        if (loginSuccessful == true)
        {
            // Load main menu scene once all Module Questions have been fetched
            SceneManager.LoadScene("MainMenu");
        }
    }

    // Manual login 
    public void LoginAttempt()
    {
        if (username.text != "" && password.text != "")
        {
            Debug.Log("Login Attempted");
            loginPending = true;
            BackendHook.StartHookWithCallback(BackendHook.Login(username.text, passwordInputfield.text), OnLoginComplete);
        }
    }

    // Triggers after the above BackendHook.Login() Coroutine finishes
    public void OnLoginComplete()
    {
        loginPending = false;

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
