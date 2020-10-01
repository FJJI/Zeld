using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UserLogin : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference reference;
    private FirebaseDatabase dbInstance;
    public InputField UserNameInput, PasswordInput, RePasswordInput, EmailInput;
    public Button SignupButton, LoginButton, CreateButton, BackButton;
    public Text ErrorText, RePassWordLabel, EmailLabel;


    void Start()
    {

        Debug.Log("started");
        auth = FirebaseAuth.DefaultInstance;

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://zeld-e907d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        dbInstance = FirebaseDatabase.DefaultInstance;

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
        ErrorText.text = "";
    }

    public void Signup(string username, string password, string re_password, string email)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(re_password) || string.IsNullOrEmpty(email))
        {
            ErrorText.text = "Empty Values";
            return;
        }

        if (password != re_password)
        {
            ErrorText.text = "Passwords do not match";
            return;
        }

        ErrorText.text = "";
        
        dbInstance.GetReference("users").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot user in snapshot.Children)
                {
                    IDictionary dictUser = (IDictionary)user.Value;
                    if (dictUser["username"].ToString() == username)
                    {
                        return;
                    }
                    if (dictUser["email"].ToString() == email)
                    {
                        return;
                    }
                }
            }
        });
        

        User new_user = new User(username, email, password, 0, 0);
        string json = JsonUtility.ToJson(new_user);
        reference.Child("users").Push().SetRawJsonValueAsync(json);
        ErrorText.text = "User Created";

        SceneManager.LoadScene("Main Menu");

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
        dbInstance.GetReference("users").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot user in snapshot.Children)
                {
                    IDictionary dictUser = (IDictionary)user.Value;
                    if (dictUser["username"].ToString() == username && dictUser["password"].ToString() == password)
                    {
                        Debug.Log("" + user.Key + dictUser["username"] + " - " + dictUser["email"] + " - " + dictUser["password"]);
                        SceneManager.LoadScene("Main Menu");
                        break;
                    }
                }
                
            }
        });
    }

    public void GoogleAuth()
    {
        
    }



}
