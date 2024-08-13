using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserMiningDataHandler : ISubject
{
    private List<UserCard> userCards = new List<UserCard>();
    #region singleton
    private UserMiningDataHandler()
    {
        SendGetUserCardsRequest(PlayerPrefs.GetInt("UserId"));
    }

    private static UserMiningDataHandler instance;
    public static UserMiningDataHandler Instance
    {
        get
        {
            if (instance == null)
                instance = new UserMiningDataHandler();
            return instance;
        }
    }
    #endregion singleton

    private List<IObserver> observers = new List<IObserver>();

    public List<UserCard> GetUserCards()
    {
        return userCards;
    }

    public void UpgradeCard(int cardId, int userId)
    {
        var req = new HttpRequest<HTTPResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/upgradeCard", (response) =>//onDone
        {
            SendGetUserCardsRequest(userId);
        }, "cardId=" + cardId + "&userId=" + userId));
    }

    public void BuyCard(int cardId, int userId)
    {
        var req = new HttpRequest<HTTPResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/buyCard", (response) =>//onDone
        {
            SendGetUserCardsRequest(userId);
        }, "cardId=" + cardId + "&userId=" + userId));
    }


    private void SendGetUserCardsRequest(int userId)
    {
        var req = new HttpRequest<GetUserCardsResponse>();
        ContextManager.Instance.StartCoroutine(req.SendRequest(ServerConfig.baseURL + "/getUserCards", (response) =>//onDone
        {
            this.userCards = response.userCards;
            Notify();
        }, "userId=" + userId));
    }

    public void Attach(IObserver observer)
    {
        observers.Add(observer);
        InitializeObserver(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void InitializeObserver(IObserver observer)
    {
        Notify();
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.UpdateObserver(this);
        }
    }

}
