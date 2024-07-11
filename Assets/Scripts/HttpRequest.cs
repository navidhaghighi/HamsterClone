using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequest<T>
{

    public IEnumerator SendRequest(string url,Action<T> onDone)
    {
        UnityWebRequest req = new UnityWebRequest();
        req.url = url;
        req.certificateHandler = new BypassCertificate();
        req.downloadHandler = new DownloadHandlerBuffer();
        yield return req.SendWebRequest();
        T response =  JsonUtility.FromJson<T>(req.downloadHandler.text);
        onDone?.Invoke(response);
    }

}

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        //Simply return true no matter what
        return true;
    }
}
