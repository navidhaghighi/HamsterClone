using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObserver
{
    // Receive update from subject
    void UpdateObserver(ISubject subject);
}
