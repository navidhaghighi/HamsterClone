using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class OnEnableAnimation<T> : MonoBehaviour
{
    //play reverse OnDisable?
    [SerializeField]
    protected bool playReverse;
    [SerializeField]
    protected float firstTimeDelay;
    protected T m_Animation;
    // Start is called before the first frame update
    void Awake()
    {
        m_Animation = GetComponent<T>();
    }
    private void OnEnable()
    {
        StartCoroutine(PlayAnimation());
    }

    private void OnDisable()
    {
        if(playReverse)
            PlayReverse();
    }

    protected IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(firstTimeDelay);
        Play();

    }

    protected virtual void Play()
    {

    }

    protected virtual void PlayReverse()
    {

    }


}
