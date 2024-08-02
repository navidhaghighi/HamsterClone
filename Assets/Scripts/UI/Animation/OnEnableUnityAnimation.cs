using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableUnityAnimation : OnEnableAnimation<Animation>
{
    protected override void Play()
    {
        base.Play();
        m_Animation.Play();
    }
}
