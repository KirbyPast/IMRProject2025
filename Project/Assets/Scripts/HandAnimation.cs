using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandAnimation : MonoBehaviour
{
    private Animator Animator;
    public string GrabAnimationName, TriggerAnimationName;


    private void Awake()
    {
        Animator = GetComponent<Animator>();
    }

    public void GrabAnimation()
    {
        Animator.SetBool(GrabAnimationName, true);
    }


    public void TriggerAnimation()
    {
        Animator.SetBool(TriggerAnimationName, true);
    }

    public void CancelGrabAnimation()
    {
        Animator.SetBool(GrabAnimationName, false);
    }

    public void CancelTriggerAnimation()
    {
        Animator.SetBool(TriggerAnimationName, false);
    }
}
