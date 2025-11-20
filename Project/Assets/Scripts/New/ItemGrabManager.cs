using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemGrabManager : MonoBehaviour
{
    public List<XRGrabInteractable> CurrentItems = new();
    public Action<XRGrabInteractable> OnItemGrabbed, OnItemDropped;

    public void AddItem(XRGrabInteractable item)
    {
        CurrentItems.Add(item);
        OnItemGrabbed?.Invoke(item);
    }

    public void RemoveItem(XRGrabInteractable item)
    {
        CurrentItems.Remove(item);
        OnItemDropped?.Invoke(item);
    }

    public bool HasType<T>(out XRGrabInteractable item) where T : Component
    {
        item = CurrentItems.Find(it => it.GetComponent<T>() != null);
        if (item == null)
        {
            return false;
        }
        return true;
    }

}
