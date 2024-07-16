using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLoginReq : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var req =  new HttpRequest<User>();
       StartCoroutine( req.SendRequest(ServerConfig.baseURL + "/login", (response) =>
        {

        },"id=25"));
    }

    
}
