using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance;

    public ItemGrabManager itemGrabManager;
    public static ItemGrabManager ItemGrabManager => Instance.itemGrabManager;

    public ViewPcComponentUI viewPcComponentUI;
    public static ViewPcComponentUI ViewPcComponentUI => Instance.viewPcComponentUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
