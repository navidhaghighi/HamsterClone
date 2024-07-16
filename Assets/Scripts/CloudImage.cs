using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
[RequireComponent(typeof(RawImage))]
public class CloudImage : MonoBehaviour
{
    private RawImage image;
    void Awake()
    {
        image = GetComponent<RawImage>();    
    }

    public void Init(string url)
    {
        StartCoroutine(setImage(ServerConfig.baseURL+"/images/"+ url));
    }

    IEnumerator setImage(string url)
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Texture myTexture = ((DownloadHandlerTexture)www.downloadHandler).texture;
            image.texture = myTexture;
        }
    }
}
