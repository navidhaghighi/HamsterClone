using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HTTPResponse
{
    //success or failure
    public string responseResult;
    //what error did server respond with(if any)
    public string errorMessage;
}    
