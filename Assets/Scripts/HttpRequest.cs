using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class HttpRequest<T>
{

    public IEnumerator SendRequest(string url,Action<T> onDone, string postParams="")
    {

        if (string.IsNullOrEmpty(postParams) == false)
        {

           // WWWForm formData = new WWWForm();
           // formData.AddField("username", "Barodar");
            UnityWebRequest req =  new UnityWebRequest(url, UnityWebRequest.kHttpVerbPOST);
            req.uploadHandler = new UploadHandlerRaw(System.Text.UTF8Encoding.UTF8.GetBytes(postParams));
            req.uploadHandler.contentType = "application/x-www-form-urlencoded";
            req.certificateHandler = new BypassCertificate();
            req.downloadHandler = new DownloadHandlerBuffer();
             req.SendWebRequest();
            while (req.downloadProgress<1)
            {
                Debug.LogWarning("Req progress "+ req.downloadProgress);
                yield return null;
            }
            Debug.LogWarning("Req error is "+ req.error);
            //check for error messages inside the response 
            HTTPResponse serverResponse = JsonUtility.FromJson<HTTPResponse>(req.downloadHandler.text);
            if(serverResponse != null )
            {
                if(serverResponse.responseResult=="failure")
                    Debug.LogError("Received this error from server "+ serverResponse.errorMessage);
            }    
            T genericResponse = JsonUtility.FromJson<T>(req.downloadHandler.text);
            onDone?.Invoke(genericResponse);
        }
        else
        {
            UnityWebRequest req = new UnityWebRequest(url);
            req.certificateHandler = new BypassCertificate();
            req.downloadHandler = new DownloadHandlerBuffer();
            yield return req.SendWebRequest();
            T response = JsonUtility.FromJson<T>(req.downloadHandler.text);
            onDone?.Invoke(response);
        }

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
