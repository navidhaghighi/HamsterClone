using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningDataHandler : ISubject
{
    private List<Card> _cards = new List<Card>();
    #region singleton
    private MiningDataHandler()
    {
        SendGetCardsRequest();
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

    private void SendGetCardsRequest()
    {
        var req = new HttpRequest<GetCardsResponse>();
       ContextManager.Instance.StartCoroutine(req.SendRequest("http://localhost:3000/GetMiningCards", (response) =>//onDone
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
