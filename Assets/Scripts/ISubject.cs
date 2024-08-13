using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    // Attach an observer to the subject.
    void Attach(IObserver observer);

    // Detach an observer from the subject.
    void Detach(IObserver observer);

    // Notify all observers about an event.
    void Notify();

    void InitializeObserver(IObserver observer);
}
