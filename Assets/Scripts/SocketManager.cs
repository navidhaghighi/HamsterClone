using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using WebSocketSharp;

public  class SocketManager : MonoBehaviour,IObserver
{
    private User currentUser;
    private bool init;
    public  Action<string> onMessage;
    public WebSocket ws;


    private void Start()
    {
        Invoke( "InitializeSocket",2f);
    }

    private void OnDestroy()
    {
        ws.Send(JsonUtility.ToJson( new UserSocketMessage() { messageType = "ConnectionClose", user = currentUser }));
    }

    private void InitializeSocket()
    {
        ws = new WebSocket("ws://127.0.0.1:8080");

        ws.OnMessage += Ws_OnMessage;

        ws.Connect();
        Console.ReadKey(true);
        init = true;
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.LogWarning("Scoket message received message "+ e.Data.ToString());
        onMessage?.Invoke(e.Data);
    }
    [ContextMenu("SendHello")]
    public void SendTest()
    {
        StartCoroutine(SendMessageViaSocket("Hello"));
    }

    public IEnumerator SendMessageViaSocket(string msg)
    {
        yield return new WaitUntil(()=>init== true);
        ws.Send(msg);
    }

    public void UpdateObserver(ISubject subject)
    {
        if (subject is UserDataHandler)
        {
            UserDataHandler userData = (UserDataHandler)subject;
            currentUser = userData.currentUser;
        }
    }
}

[System.Serializable]
public class UserSocketMessage : WebSocketMessage
{
    public User user;
}
[System.Serializable]
public class CoinUpdateMessage : WebSocketMessage
{
    public int coin;
}