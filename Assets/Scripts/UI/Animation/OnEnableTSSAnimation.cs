using System.Collections;
using System.Collections.Generic;
using System.Data;
using TSS;
using UnityEngine;

public class OnEnableTSSAnimation : OnEnableAnimation<TSSItem>
{
    protected override void Play()
    {
        base.Play();
        m_Animation.Open();
    }

    protected override void PlayReverse()
    {
        base.PlayReverse();
        m_Animation.Close();
    }

}
