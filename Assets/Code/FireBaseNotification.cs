using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBaseNotification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Firebase.Messaging.FirebaseMessaging.SubscribeAsync("/requestLists/"+PlayerPrefs.GetString("UID"));
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received Registration Token: " + e.Message.From);
    }
}
