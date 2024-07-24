using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDataHandler : ISubject
{
    private List<UserCard> userCards;
    private List<Card> _cards = new List<Card>();
    #region singleton
    private MiningDataHandler()
    {
        SendGetCardsRequest();
        SendGetUserCardsRequest(PlayerPrefs.GetInt("UserId"));
    }

    private static MiningDataHandler instance;
    public static MiningDataHandler Instance
    {
        get
        {
            if (instance == null)
                instance = new MiningDataHandler();
            return instance;
        }
    }
    #endregion singleton

    private List<IObserver> observers = new List<IObserver>();
    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void UpgradeCard(int cardId, int userId)
    {
        var req = new HttpRequest<HTTPResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/upgradeCard", (response) =>//onDone
        {
            SendGetUserCardsRequest(userId);
        }, "cardId=" + cardId + "&userId=" + userId));
    }

    public void BuyCard(int cardId,int userId)
    {
        var req = new HttpRequest<HTTPResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/buyCard", (response) =>//onDone
        {
            SendGetUserCardsRequest(userId);
        }, "cardId=" + cardId+"&userId="+userId));
    }

    private void SendGetUserCardsRequest(int userId)
    {
        var req = new HttpRequest<GetUserCardsResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/getUserCards", (response) =>//onDone
        {
           // this.userCards = response;
            Notify();
        }, "userId=" + userId));
    }

    public List<UserCard> GetUserCards()
    {
        return userCards;
    }

    private void SendGetCardsRequest()
    {
        var req = new HttpRequest<GetCardsResponse>();
       ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL+"/getMiningCards", (response) =>//onDone
        {
            this._cards = response.cards;
            Notify();
        }));
    }

    public List<Card> GetCards()
    {
        return _cards;
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.UpdateObserver(this);
        }
    }
}
