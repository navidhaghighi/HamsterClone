using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultListener : ISubject
{
    private List<IObserver> observers;
    public void Attach(IObserver observer)
    {
        observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }
}
