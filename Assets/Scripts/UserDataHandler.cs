using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserDataHandler : ISubject
{
    private int coinsAmount;
    private List<IObserver<T>> observers;
    public void Attach(IObserver<T> observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void IncreaseCoins(int  amount)
    {
        coinsAmount += amount;
        Notify();
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update();
        }
    }
}
