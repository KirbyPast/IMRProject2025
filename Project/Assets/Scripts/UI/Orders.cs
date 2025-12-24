using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public PurchasedOrderUI OrginalPurchasedOrderUI;
    public List<PurchasedOrderUI> AllPurchasedOrderUI = new();
    public void CreateOrder(PcComponent component, Action onDelivered, float time = 10)
    {
        var newOrder = Instantiate(OrginalPurchasedOrderUI, OrginalPurchasedOrderUI.transform.parent);
        newOrder.gameObject.SetActive(true);
        newOrder.Create(component, onDelivered, time);
    }
}
