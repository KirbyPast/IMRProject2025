using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public ComponentUI originalComponent;
    public List<ComponentUI> allComponents = new();


    private void Start()
    {
        foreach(var comp in Storage.Components)
        {
            InstantiatComponentUI(comp);
        }
    }

    public void InstantiatComponentUI(PcComponent pc)
    {
        var cmp = Instantiate(originalComponent, originalComponent.transform.parent);
        cmp.Create(pc);
        cmp.gameObject.SetActive(true);
    }
}
