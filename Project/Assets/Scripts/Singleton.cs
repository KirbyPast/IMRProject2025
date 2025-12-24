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

    public DetailsTab detailsTab;
    public static DetailsTab DetailsTab => Instance.detailsTab;

    public GameObject player;
    public static GameObject Player => Instance.player;

    public Orders orders;
    public static Orders Orders => Instance.orders;

    public Shop shop;
    public static Shop Shop => Instance.shop;


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
