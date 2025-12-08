using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public PcComponentUI originalComponent;
    public List<PcComponentUI> allComponents = new();
    public GameObject originalPhisicalPcComponent; //Case, Cooler, Cpu, Gpu, Motherboard, Psu, Ram, Storage
    public TMP_Dropdown D_Types, D_Order;
    public PriceInfo PI_Min, PI_Max;

    private void Start()
    {
        print("Instantiating components");
        foreach(var comp in Storage.Components)
        {
            InstantiatComponentUI(comp);
        }

        D_Types.onValueChanged.AddListener(val =>
        {
            Type componentType = val switch
            {
                1 => typeof(Case),
                2 => typeof(Cooler),
                3 => typeof(Cpu),
                4 => typeof(Gpu),
                5 => typeof(MotherBoard),
                6 => typeof(Psu),
                7 => typeof(Ram),
                8 => typeof(Storage),
                _ => typeof(PcComponent)

            };

            foreach(var comp in allComponents)
            {
                comp.gameObject.SetActive(componentType.IsInstanceOfType(comp.thisComponent));
            }

        });

        D_Order.onValueChanged.AddListener(val =>
        {
            Transform parent = originalComponent.transform.parent;

            List<Transform> children = new();
            foreach (Transform child in parent)
                children.Add(child);

            switch (val)
            {
                case 0:
                    children = children
                        .OrderBy(c => c.GetComponent<PcComponentUI>().thisComponent.ModelId)
                        .ToList();
                    break;

                case 1:
                    children = children
                        .OrderBy(c => c.GetComponent<PcComponentUI>().thisComponent.Price)
                        .ToList();
                    break;

                case 2:
                    children = children
                        .OrderByDescending(c => c.GetComponent<PcComponentUI>().thisComponent.Price)
                        .ToList();
                    break;

                case 3:
                    children = children
                        .OrderBy(c => c.GetComponent<PcComponentUI>().thisComponent.Name)
                        .ToList();
                    break;
            }

            for (int i = 0; i < children.Count; i++)
                children[i].SetSiblingIndex(i);
        });

        PI_Min.OnChange += (val) => { FilterPrice(val, PI_Max.Price); };
        PI_Max.OnChange += (val) => { FilterPrice(PI_Min.Price, val); };
    }

    public void InstantiatComponentUI(PcComponent pc)
    {
        var cmp = Instantiate(originalComponent, originalComponent.transform.parent);
        cmp.Create(this, pc);
        cmp.gameObject.SetActive(true);
        allComponents.Add(cmp);
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
        newPhisicalPcComponent.SetActive(true);
    }

    public void FilterPrice(float min, float max)
    {
        foreach (var comp in allComponents)
        {
            comp.gameObject.SetActive(comp.thisComponent.Price >= min && comp.thisComponent.Price <= max);
        }
    }    
}
