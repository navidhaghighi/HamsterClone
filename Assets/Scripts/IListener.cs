using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject<T>
{

    void Attach(IObserver<T> observer);
    void Detach(IObserver<T> observer);

    void Notify();
}
