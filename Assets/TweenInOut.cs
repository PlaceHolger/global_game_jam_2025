using System;
using PrimeTween;
using UnityEngine;

public class TweenInOut : MonoBehaviour
{
    float m_FadeTime = 1;
    float m_StayTime = 1;

    private void OnEnable()
    {
        //Tween.Scale(transform, 1, m_FadeTime).OnComplete().Scale(1, m_StayTime).OnComplete().Scale(0, m_FadeTime);
    }
}
