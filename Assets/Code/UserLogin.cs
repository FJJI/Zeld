using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UserLogin : MonoBehaviour
{
    private FirebaseAuth auth;
    public InputField UserNameInput, PasswordInput, RePasswordInput, EmailInput;
    public Button SignupButton, LoginButton, CreateButton, BackButton;
    public Text ErrorText, RePassWordLabel, EmailLabel;

    void Start()
    {
        Debug.Log("started");
        auth = FirebaseAuth.DefaultInstance;
        CreateButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
        RePasswordInput.gameObject.SetActive(false);
        RePassWordLabel.gameObject.SetActive(false);
        EmailLabel.gameObject.SetActive(false);
        EmailInput.gameObject.SetActive(false);

        LoginButton.onClick.AddListener(() => Login(UserNameInput.text, PasswordInput.text));
        SignupButton.onClick.AddListener(() => DisplayForm());
        CreateButton.onClick.AddListener(() => Signup(UserNameInput.text, PasswordInput.text, RePasswordInput.text, EmailInput.text));
        BackButton.onClick.AddListener(() => DisplayLogin());
        
    }

    public void DisplayForm()
    {
        CreateButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        RePasswordInput.gameObject.SetActive(true);
        RePassWordLabel.gameObject.SetActive(true);
        EmailLabel.gameObject.SetActive(true);
        EmailInput.gameObject.SetActive(true);
        SignupButton.gameObject.SetActive(false);
        LoginButton.gameObject.SetActive(false);
    }

    public void DisplayLogin()
    {
        CreateButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
        RePasswordInput.gameObject.SetActive(false);
        RePassWordLabel.gameObject.SetActive(false);
        EmailLabel.gameObject.SetActive(false);
        EmailInput.gameObject.SetActive(false);
        SignupButton.gameObject.SetActive(true);
        LoginButton.gameObject.SetActive(true);
    }

    public void Signup(string username, string password, string re_password, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(re_password) || string.IsNullOrEmpty(email))
        {
            //Error handling
            return;
        }

        if (password != re_password)
        {
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser newUser = task.Result; // Firebase user has been created.
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            UpdateErrorMessage("Signup Success");
        });
    }

    private void UpdateErrorMessage(string message)
    {
        ErrorText.text = message;
        Invoke("ClearErrorMessage", 3);
    }

    void ClearErrorMessage()
    {
        ErrorText.text = "";
    }

    public void Login(string username, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(username, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
                if (task.Exception.InnerExceptions.Count > 0)
                    UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
                return;
            }

            FirebaseUser user = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                user.DisplayName, user.UserId);

            PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
            //SceneManager.LoadScene("LoginResults");
        });
    }

    public void GoogleAuth()
    {
        
    }



}
