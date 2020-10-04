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
    public Button FriendListButton, ProfileButton, CreateRoomButton, AccessRoomButton, RequestsButton, FriendsButton, AddFriend, BackButton;
    public GameObject FriendsView, RequestsView;
    public GameObject FriendRow, RequestRow;
    public Transform FriendContent, RequestContent;
    public Text ErrorMessage;
    public List<Request> requestsL;
    public List<Friend> friendsL;
    bool found;
    long rows;
    public string logged_key, target;
    private DatabaseReference reference;
    private FirebaseDatabase dbInstance;

    
    void Start()
    {
        found = false;
        target = "";
        rows = 0;
        requestsL = new List<Request>();
        friendsL = new List<Friend>();
        logged_key = PlayerPrefs.GetString("UID");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://zeld-e907d.firebaseio.com/");
        reference = FirebaseDatabase.DefaultInstance.RootReference; //escritura
        dbInstance = FirebaseDatabase.DefaultInstance; //lectura
        ProfileButton.onClick.AddListener(() => DisplayProfile());
        FriendListButton.onClick.AddListener(() => DisplayFriends(logged_key));
        FriendsButton.onClick.AddListener(() => DisplayFriends(logged_key));
        RequestsButton.onClick.AddListener(() => DisplayRequests(logged_key));
        BackButton.onClick.AddListener(() => Back());


    }

    public async void DisplayFriends(string key)
    {
        FriendListButton.gameObject.SetActive(false);
        ProfileButton.gameObject.SetActive(false);
        CreateRoomButton.gameObject.SetActive(false);
        AccessRoomButton.gameObject.SetActive(false);
        RoomCode.gameObject.SetActive(false);
        RequestsButton.gameObject.SetActive(true);
        FriendsButton.gameObject.SetActive(true);
        BackButton.gameObject.SetActive(true);
        FriendToAdd.gameObject.SetActive(true);
        FriendsView.gameObject.SetActive(true);
        // obtenemos todos los amigos de la lista
        //Primero Consultamos a la Base de datos por nuestra lista de amigos
        await dbInstance.GetReference("friendLists").Child(key).Child("friends").GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) { }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Vemos si es que tenemos amigos
                rows = snapshot.ChildrenCount;
                // Si hay amigos, los agregamos a una lista que usaremos para las rondas.
                if (rows != 1)
                {
                    foreach (DataSnapshot friend in snapshot.Children)
                    {
                        IDictionary dictFriend = (IDictionary)friend.Value;
                        Friend f_row = new Friend(dictFriend["friend_name"].ToString(), dictFriend["friend_id"].ToString(), 0, 0);
                        friendsL.Add(f_row);
                    }
                }

            }
        });
        // Aqui empezamos el ciclo para agregar cada row al FriendsView
        for (int i = 0; i < friendsL.Count; i++)
        {
            GameObject SpawnedItem = Instantiate(FriendRow);
            SpawnedItem.transform.SetParent(FriendContent, false);
            SpawnedItem.transform.GetChild(0).GetComponent<Text>().text = friendsL[i].friend_name;
            SpawnedItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => DeleteFriend(logged_key,friendsL[i].friend_id));

        }

    }

    public async void DisplayRequests(string key)
    {
        FriendsView.gameObject.SetActive(false);
        RequestsView.gameObject.SetActive(true);
        // obtenemos todas las solicitudes recibidas en nuestra lista de requests
        //Primero Consultamos a la Base de datos por nuestra lista de solicitudes
        await dbInstance.GetReference("requestLists").Child(key).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) { }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                // Vemos si es que tenemos solicitudes
                rows = snapshot.ChildrenCount;
                // Si hay solicitudes, las agregamos a una lista que usaremos para las rondas.
                if (rows != 1)
                {
                    foreach (DataSnapshot request in snapshot.Children)
                    {
                        IDictionary dictRequest = (IDictionary)request.Value;
                        Request r_row = new Request(dictRequest["from_id"].ToString(), dictRequest["from_name"].ToString(), dictRequest["target_id"].ToString());
                        requestsL.Add(r_row);
                    }
                }
                else
                {
                    return;
                }
                
            }    
        });
        // Aqui empezamos el ciclo para agregar cada row al requestsView
        
        for (int i = 0; i < requestsL.Count; i++)
        {
            GameObject SpawnedItem = Instantiate(RequestRow);
            SpawnedItem.transform.SetParent(RequestContent, false);
            SpawnedItem.transform.GetChild(0).GetComponent<Text>().text = requestsL[i].from_name;
            SpawnedItem.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => AcceptFriend(logged_key, requestsL[i].from_name, requestsL[i].from_id));
            SpawnedItem.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => RefuseRequest(logged_key,  requestsL[i].from_id));
        }

    }

    public async void AddFriends(string key, string friend_code)
    {
        //write to target user's request list
        await dbInstance.GetReference("users").Child(friend_code).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) { }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot == null)
                {
                    found = false;
                    Debug.Log("not founded");
                }
                else
                {
                    found = true;
                    IDictionary dictUser = (IDictionary)snapshot.Value;
                    target = dictUser["username"].ToString();
                    Debug.Log("founded");
                }

            }
        });

        if (found == false)
        {
            ErrorMessage.text = "User not Found";
        }
        else
        {
            ErrorMessage.text = "Resquest sended";
            Request request = new Request(logged_key, target, friend_code);
            string json = JsonUtility.ToJson(request);
            await reference.Child("requestLists").Child(friend_code).Child(logged_key).SetRawJsonValueAsync(json);

        }

    }

    public async void AcceptFriend(string key, string name, string friend_key)
    {
        await reference.Child("requestLists").Child(key).Child(friend_key).RemoveValueAsync();
        Friend friend1 = new Friend(name, friend_key, 0, 0); //friend in my list.
        Friend friend2 = new Friend(PlayerPrefs.GetString("Username"), key, 0, 0); //friend in his list.
        string json = JsonUtility.ToJson(friend1);
        await reference.Child("friendLists").Child(key).Child("friends").Child(friend_key).SetRawJsonValueAsync(json);
        json = JsonUtility.ToJson(friend2);
        await reference.Child("friendLists").Child(friend_key).Child("friends").Child(key).SetRawJsonValueAsync(json);
    }

    public void RefuseRequest(string key, string friend_key)
    {
        reference.Child("requestLists").Child(key).Child(friend_key).RemoveValueAsync();
    }

    public void DeleteFriend(string key, string friend_key)
    {

    }

    public void Back()
    {
        FriendListButton.gameObject.SetActive(true);
        ProfileButton.gameObject.SetActive(true);
        CreateRoomButton.gameObject.SetActive(true);
        AccessRoomButton.gameObject.SetActive(true);
        RoomCode.gameObject.SetActive(true);
        FriendToAdd.gameObject.SetActive(false);
        RequestsButton.gameObject.SetActive(false);
        FriendsButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(false);
        FriendToAdd.gameObject.SetActive(false);
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
