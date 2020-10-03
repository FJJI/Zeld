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
    public InputField RoomCode;
    public Button FriendListButton, ProfileButton, CreateRoomButton, AccessRoomButton;

    public string logged_key;
    private DatabaseReference reference;
    private FirebaseDatabase dbInstance;
    // Start is called before the first frame update
    void Start()
    {
        logged_key = null;
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://zeld-e907d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        dbInstance = FirebaseDatabase.DefaultInstance;

        ProfileButton.onClick.AddListener(() => DisplayProfile());
        FriendListButton.onClick.AddListener(() => DisplayFriends(logged_key));

    }

    public void DisplayFriends(string key)
    {

    }

    public void Back()
    {

    }

    public void DisplayProfile()
    {

    }
}
