﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class RoomScript : MonoBehaviour
{

    public Button readyButton, startButton, leaveButton, sendButton;
    public Text idLabel, playersLabel;
    public InputField chatInput;
    public GameObject chatView, playersView, chatRow, playersRow;
    public Transform chatContent, playersContent;
    private DatabaseReference reference;
    private FirebaseDatabase dbInstance;
    public string logged_key;
    public string roomId, errorMessage;
    public List<PlayerClass> room_players;
    public List<int> id_players;
    public bool ready;
    public long capacity;

    void Start()
    {
        logged_key = PlayerPrefs.GetString("UID");
        roomId = PlayerPrefs.GetString("Room");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://zeld-e907d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference; //escritura
        dbInstance = FirebaseDatabase.DefaultInstance; //lectura
        room_players = new List<PlayerClass>();
        capacity = 0;
        ready = false;
        errorMessage = "";
        SetupDisplay();
        dbInstance.GetReference("rooms").Child(roomId).Child("messages").ChildAdded += HandleMessageAdded;
        dbInstance.GetReference("rooms").Child(roomId).Child("players").ChildAdded += HandlePLayerAdded;
        dbInstance.GetReference("rooms").Child(roomId).Child("players").ChildRemoved += HandlePlayerRemoved;
        sendButton.onClick.AddListener(() => SenderChat(chatInput.text));
        leaveButton.onClick.AddListener(() => LeavePress());
        readyButton.onClick.AddListener(() =>ReadyPress());
    }

    void HandleMessageAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot msg = args.Snapshot;
        IDictionary dictMsg = (IDictionary)msg.Value;
        Message m_row = new Message(dictMsg["from"].ToString(), dictMsg["content"].ToString());
        GameObject SpawnedItem = Instantiate(chatRow);
        SpawnedItem.transform.SetParent(chatContent, false);
        SpawnedItem.transform.GetChild(0).GetComponent<Text>().text = m_row.from;
        SpawnedItem.transform.GetChild(1).GetComponent<Text>().text = m_row.content;
    }

    void HandlePLayerAdded(object sender, ChildChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        DataSnapshot data = args.Snapshot;
        IDictionary dictPlayer = (IDictionary)data.Value;
        PlayerClass p_row = new PlayerClass(dictPlayer["username"].ToString(), dictPlayer["ready"].ToString());
        GameObject SpawnedItem = Instantiate(playersRow);
        SpawnedItem.transform.SetParent(playersContent, false);
        SpawnedItem.transform.GetChild(0).GetComponent<Text>().text = p_row.username;
        SpawnedItem.transform.GetChild(1).GetComponent<Text>().text = p_row.ready.ToString();
    }

    void HandlePlayerRemoved(object sender, ChildChangedEventArgs args)
    {
         
    }

    public async void SetupDisplay()
    {
        await dbInstance.GetReference("rooms").Child(roomId).GetValueAsync().ContinueWith(task =>
        {
            DataSnapshot snapshot = task.Result;
            IDictionary room = (IDictionary)snapshot.Value;
            capacity = long.Parse(room["roomSize"].ToString());
        });
        await dbInstance.GetReference("rooms").Child(roomId).Child("players").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted) { }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot p in snapshot.Children)
                {
                    IDictionary dictPlayer = (IDictionary)p.Value;
                    PlayerClass p_row = new PlayerClass(dictPlayer["username"].ToString(), dictPlayer["ready"].ToString());
                    room_players.Add(p_row);
                }

            }
        });

        for (int i = 0; i < room_players.Count; i++)
        {
            GameObject SpawnedItem = Instantiate(playersRow);
            SpawnedItem.transform.SetParent(playersContent, false);
            SpawnedItem.transform.GetChild(0).GetComponent<Text>().text = room_players[i].username;
            SpawnedItem.transform.GetChild(1).GetComponent<Text>().text = room_players[i].ready.ToString();
        }
    }

    public async void ReadyPress()
    {
        await reference.Child("rooms").Child(roomId).Child("players").Child(logged_key).Child("ready").SetValueAsync("true");
    }

    public async void LeavePress()
    {
        await reference.Child("rooms").Child(roomId).Child("players").Child(logged_key).RemoveValueAsync();
        SceneManager.LoadScene("Main Menu");
    }

    public async void StartGame()
    {
        await dbInstance.GetReference("rooms").Child(roomId).Child("players").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted) { }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.ChildrenCount != capacity)
                {
                    errorMessage = "Not enough players";
                    return;
                   
                }
                foreach (DataSnapshot p in snapshot.Children)
                {
                    IDictionary dictPlayer = (IDictionary)p.Value;
                    if (dictPlayer["ready"].ToString() == "false")
                    {
                        ready = false;
                        errorMessage = "All players must be ready";
                        break;
                    }
                    else
                    {
                        ready = true;
                    }
                    
                }
            }
        });
        if (ready == false)
        {
            //display message; 
            return;   
        }
        //start game
    }

    public async void SenderChat(string content)
    {
        chatInput.text = "";
        Message message = new Message(PlayerPrefs.GetString("UserName"), content);
        string json = JsonUtility.ToJson(message);
        await reference.Child("rooms").Child(roomId).Child("messages").Push().SetRawJsonValueAsync(json);
    }

    
}
