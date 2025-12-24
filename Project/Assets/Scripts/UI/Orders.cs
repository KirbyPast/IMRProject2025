using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Orders : MonoBehaviour
{
    public PurchasedOrderUI OrginalPurchasedOrderUI;
    public List<PurchasedOrderUI> AllPurchasedOrderUI = new();

    public SoldOrderUI OriginalSoldOrderUI;
    public List<SoldOrderUI> AllSoldOrderUI = new();

    public void CreateOrder(PcComponent component, Action onDelivered, float time = 10)
    {
        var newOrder = Instantiate(OrginalPurchasedOrderUI, OrginalPurchasedOrderUI.transform.parent);
        AllPurchasedOrderUI.Add(newOrder);

        newOrder.transform.SetAsFirstSibling();
        newOrder.gameObject.SetActive(true);
        newOrder.Create(component, onDelivered, time);
    }

    public void CreateSoldOrder(List<PhisicalPcComponent> components)
    {
        var newOrder = Instantiate(OriginalSoldOrderUI, OriginalSoldOrderUI.transform.parent);
        AllSoldOrderUI.Add(newOrder);

        newOrder.transform.SetAsFirstSibling();
        newOrder.gameObject.SetActive(true);
        newOrder.Create(components, AllSoldOrderUI.Count);
    }
}
