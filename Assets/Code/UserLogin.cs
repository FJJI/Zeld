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
    public string logged_key;
    private FirebaseAuth auth;
    private DatabaseReference reference;
    private FirebaseDatabase dbInstance;
    public InputField UserNameInput, PasswordInput, RePasswordInput, EmailInput;
    public Button SignupButton, LoginButton, CreateButton, BackButton;
    public Text ErrorText, RePassWordLabel, EmailLabel;
    public bool mail, uname;


    void Start()
    {

        Debug.Log("started");
        auth = FirebaseAuth.DefaultInstance;
        logged_key = null;
        mail = false;
        uname = false;
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
        BackButton.onClick.AddListener(() => DisplayLogin(" "));
        
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

    public void DisplayLogin(string message)
    {
        CreateButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
        RePasswordInput.gameObject.SetActive(false);
        RePassWordLabel.gameObject.SetActive(false);
        EmailLabel.gameObject.SetActive(false);
        EmailInput.gameObject.SetActive(false);
        SignupButton.gameObject.SetActive(true);
        LoginButton.gameObject.SetActive(true);
        ErrorText.text = message;
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
                        uname = true;
                        break;
                    }
                    if (dictUser["email"].ToString() == email)
                    {
                        mail = true;
                        break;
                    }
                }
            }
        });

        Wait(2, () => {

            if (uname) {
                ErrorText.text = "Username Already Exists";
                return; 
            }

            if (mail) {
                ErrorText.text = "Email Already in the System";
            }

            User new_user = new User(username, email, password, 0, 0);
            string json = JsonUtility.ToJson(new_user);
            reference.Child("users").Push().SetRawJsonValueAsync(json);

            DisplayLogin("User Created");
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
        dbInstance.GetReference("users").GetValueAsync().ContinueWith(task =>
        {
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
                        Debug.Log("" + user.Key + "-" + dictUser["username"] + " - " + dictUser["email"] + " - " + dictUser["password"]);
                        logged_key = user.Key;
                        break;
                        
                    }
                }

            }
        });
        Wait(2, () => {
            Debug.Log(logged_key);
            if (logged_key != null)
            {
                SceneManager.LoadScene("Main Menu");
            }
        });
        


    }

    public void GoogleAuth()
    {
        
    }

    public void Wait(float seconds, Action action)
    {
        StartCoroutine(_wait(seconds, action));
    }

    IEnumerator _wait(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        callback();
    }

}
