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

public class MainMenu : MonoBehaviour
{
    public InputField RoomCode, FriendToAdd;
    public Button FriendListButton, ProfileButton, CreateRoomButton, AccessRoomButton, RequestsButton, FriendsButton, AddFriend;
    public GameObject FriendList, ContentView;
    public Text ErrorMessage;
    bool found;

    public string logged_key;
    private DatabaseReference reference;
    private FirebaseDatabase dbInstance;
    // Start is called before the first frame update
    void Start()
    {
        found = false;
        logged_key = PlayerPrefs.GetString("UID");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://zeld-e907d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        dbInstance = FirebaseDatabase.DefaultInstance;
        ProfileButton.onClick.AddListener(() => DisplayProfile());
        FriendListButton.onClick.AddListener(() => DisplayFriends(logged_key));

    }

    public void DisplayFriends(string key)
    {

    }

    public void DisplayRequests(string key)
    {

    }

    public void AddFriends(string key, string friend_code)
    {
        //write to target user's request list
        dbInstance.GetReference("users").Child(friend_code).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) { }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null)
                {
                    found = false;
                }
                else
                {
                    found = true;
                }

            }
        });
        Wait(2, () =>
        {
            if (found == false)
            {
                ErrorMessage.text = "User not Found";
            }
            else
            {
                ErrorMessage.text = "Resquest sended";
                Request request = new Request(logged_key, friend_code);
                string json = JsonUtility.ToJson(request);
                reference.Child("requestLists").Child(friend_code).Child("owner").Child(logged_key).SetRawJsonValueAsync(json);

            }
        });
    }




    public void Back()
    {

    }

    public void DisplayProfile()
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
