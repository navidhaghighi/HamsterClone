using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Use this script for objects that are not monobehaviours
/// </summary>
public class ContextManager : MonoBehaviour
{
    #region singleton

    private static ContextManager instance;
    public static ContextManager Instance
    {
        get
        {
            if (instance == null)
                instance = new GameObject("ContextManager").AddComponent<ContextManager>();
            return instance;
        }
    }
    #endregion singleton
}
