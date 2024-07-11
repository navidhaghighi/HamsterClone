using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteObserverA : IObserver
{
    public void UpdateObserver(ISubject subject)
    {
        if ((subject as Subject).State < 3)
        {
            Console.WriteLine("ConcreteObserverA: Reacted to the event.");
        }
    }
}
