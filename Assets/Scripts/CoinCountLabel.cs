using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;
using WebSocketSharp;
[RequireComponent(typeof(TextMeshProUGUI))]
public class CoinCountLabel :MonoBehaviour, IObserver
{
    private bool currentCoinChanged;
    private int currentCoin;
    private bool initWebsocket;
    WebSocket ws;
    private TextMeshProUGUI label;
    public void Start()
    {
        ws = new WebSocket("ws://127.0.0.1:8080");
        ws.Connect();
        ws.OnMessage += Ws_OnMessage;
        label = GetComponent<TextMeshProUGUI>();
        UserDataHandler.Instance.Attach(this);
    }

    private void OnDestroy()
    {
        ws.Close();
        UserDataHandler.Instance.Detach(this);
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        WebSocketMessage msg = JsonUtility.FromJson<WebSocketMessage>(e.Data);
        switch (msg.messageType)
        {
            case "coinsUpdate":
                CoinUpdateMessage updateMsg = JsonUtility.FromJson<CoinUpdateMessage>(e.Data);
                currentCoin = updateMsg.coin;
                currentCoinChanged = true;
                break;
            default:
                break;
        }
    }

    public void UpdateObserver(ISubject subject)
    {
        if(subject is UserDataHandler)
        {
            UserDataHandler dataHandler = (UserDataHandler)subject;
            if (initWebsocket == false)
                StartWebsocketConnection(dataHandler);
            Debug.LogWarning("Coins changed " + dataHandler.GetCoinAmount());
            label.text = dataHandler.GetCoinAmount().ToString();
        }
    }
    private void StartWebsocketConnection(UserDataHandler userData)
    {
        initWebsocket = true;
        ws.Send(JsonUtility.ToJson(new UserSocketMessage() { messageType = "UserData", user = userData.currentUser }));
    }

    private void Update()
    {
        if (currentCoinChanged)
        {
            currentCoinChanged=false;
            label.text = currentCoin.ToString();
        }
    }
}

