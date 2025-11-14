using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public PcComponentUI originalComponent;
    public List<PcComponentUI> allComponents = new();
    public GameObject originalPhisicalPcComponent; //Case, Cooler, Cpu, Gpu, Motherboard, Psu, Ram, Storage
    

    private void Start()
    {
        print("Instantiating components");
        foreach(var comp in Storage.Components)
        {
            InstantiatComponentUI(comp);
        }
    }

    public void InstantiatComponentUI(PcComponent pc)
    {
        var cmp = Instantiate(originalComponent, originalComponent.transform.parent);
        cmp.Create(this, pc);
        cmp.gameObject.SetActive(true);
    }

    public void BuyComponent(PcComponentUI pcui, PcComponent pc)
    {
        print($"Buying: {pc.Name}");
        Type componentType = pc switch
        {
            Case => typeof(PhisicalCase),
            Cooler => typeof(PhisicalCooler),
            Cpu => typeof(PhisicalCpu),
            Gpu => typeof(PhisicalGpu),
            MotherBoard => typeof(PhisicalMotherBoard),
            Psu => typeof(PhisicalPsu),
            Ram => typeof(PhisicalRam),
            Drive => typeof(PhisicalStorage),
            _ => typeof(PhisicalCase)

        };

        GenerateNewPhisicalPcComponent(componentType, pc);

    }

    public void GenerateNewPhisicalPcComponent(Type componentType, PcComponent pc)
    {
        var newPhisicalPcComponent = Instantiate(originalPhisicalPcComponent);
        newPhisicalPcComponent.AddComponent(componentType);
        newPhisicalPcComponent.GetComponent<PhisicalPcComponent>().Create(pc);
        newPhisicalPcComponent.gameObject.SetActive(true);
    }
}
