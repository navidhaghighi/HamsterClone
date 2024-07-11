using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIncreaseProfit : MonoBehaviour
{
    [ContextMenu("Increase")]
    public void IncreaseProfit()
    {
        UserDataHandler.Instance.IncreaseProfit(900);
    }

}
