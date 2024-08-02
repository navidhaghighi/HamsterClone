using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LerpScale : MonoBehaviour
{
    [SerializeField]
    private Vector3 initialScale;
    [SerializeField]
    private float animationTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lerp());
    }

    private IEnumerator Lerp()
    {
        yield return new WaitForSeconds(5f);
        float eachFrameIncrement =Time.deltaTime/animationTime;
        transform.localScale = initialScale;
        float time=0f;
        while (transform.localScale.x<1)
        {
            time += Time.deltaTime*4;
      transform.localScale = Vector3.Slerp(initialScale, new Vector3(1f, 1f, 1f), time);
    //            transform.localScale  = new Vector3(transform.localScale.x+eachFrameIncrement, transform.localScale.y + eachFrameIncrement, transform.localScale.z);
            yield return null;
        }
    }
}
