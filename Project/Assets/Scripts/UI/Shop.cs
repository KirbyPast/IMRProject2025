using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public PcComponentUI originalComponent;
    public List<PcComponentUI> allComponents = new();
    public GameObject originalPhisicalPcComponent; //Case, Cooler, Cpu, Gpu, Motherboard, Psu, Ram, Storage
    [Header("Money")]
    public TMP_Text T_Money;
    public float Money = 1000;

    [Header("Filters")]
    public TMP_Dropdown D_Types;
    public TMP_Dropdown D_Order;
    public PriceInfo PI_Min, PI_Max;
    public TMP_InputField In_Search;
    public int CurrentTypeFilter = 0;
    public string CurrentSearchFilter;
    
    [Header("Pagination")]
    public Paginator P_Paginator;
    public int PageSize = 10;

    [Header("UI Logic")]
    public ScrollRect SR_PiecesRect;
    public GameObject T_Empty;

    [Header("Orders")]
    public Orders Orders;



    private void Start()
    {
        print("Instantiating components");
        foreach(var comp in Storage.Components)
        {
            InstantiatComponentUI(comp);
        }

        D_Types.onValueChanged.AddListener(val =>
        {
            CurrentTypeFilter = val;
            FilterAll();
        });

        In_Search.onValueChanged.AddListener(val =>
        {
            CurrentSearchFilter = val;
            FilterAll();
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

        PI_Min.OnChange += (val) => { FilterAll(); };
        PI_Max.OnChange += (val) => { FilterAll(); };

        P_Paginator.OnChange += (val) => { FilterAll(); };

        FilterAll();

        ChangeMoney(0);
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
        ChangeMoney(-pc.Price);

        Orders.CreateOrder(pcui.thisComponent, () =>
        {
            GenerateNewPhisicalPcComponent(pc switch
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

            }, pc);
        }, 10);
    }

    public void GenerateNewPhisicalPcComponent(Type componentType, PcComponent pc)
    {
        var newPhisicalPcComponent = Instantiate(originalPhisicalPcComponent);
        newPhisicalPcComponent.AddComponent(componentType);
        newPhisicalPcComponent.GetComponent<PhisicalPcComponent>().Create(pc);
        newPhisicalPcComponent.SetActive(true);
    }

    void FilterAll()
    {
        SR_PiecesRect.verticalNormalizedPosition = 1;
        string search = CurrentSearchFilter?.Trim() ?? string.Empty;
        float minPrice = PI_Min.Value;
        float maxPrice = PI_Max.Value;

        Type componentType = CurrentTypeFilter switch
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

        List<PcComponentUI> filtered = new();
        foreach (var comp in allComponents)
        {
            if (MatchesAllFilters(comp, search, componentType, minPrice, maxPrice))
                filtered.Add(comp);
        }

        int total = filtered.Count;

        int pageIndex = P_Paginator.Value;
        int pageSize = PageSize;

        int start = pageIndex * pageSize;
        int end = Mathf.Min(start + pageSize, total);

        foreach (var comp in allComponents)
            comp.gameObject.SetActive(false);

        for (int i = start; i < end; i++)
            filtered[i].gameObject.SetActive(true);

        int currentOnPage = Mathf.Max(0, end - start);

        P_Paginator.UpdateCounters(currentOnPage, pageSize);

        T_Empty.SetActive(total == 0);
    }

    bool MatchesAllFilters(PcComponentUI comp, string search, Type componentType, float minPrice, float maxPrice)
    {
        var c = comp.thisComponent;

        if (!componentType.IsInstanceOfType(c))
            return false;

        if (c.Price < minPrice || c.Price > maxPrice)
            return false;

        if (!string.IsNullOrEmpty(search))
        {
            string s = search.ToLowerInvariant();

            if (!(c.Name?.ToLowerInvariant().Contains(s) == true
                  || c.Description?.ToLowerInvariant().Contains(s) == true
                  || c.Specs.Any(spec =>
                         (spec.Name?.ToLowerInvariant().Contains(s) == true) ||
                         (spec.Value?.ToLowerInvariant().Contains(s) == true))))
            {
                return false;
            }
        }

        return true;
    }

    public void ChangeMoney(float amount)
    {
        Money += amount;
        T_Money.text = "$" + Money;
    }

    public void Play()
    {
        AudioManager.Play("mclick");AudioManager.Play("mclick");
    }

    public void PlayKeyboard()
    {
        AudioManager.Play("keyboard");
    }

    public void SellAudio()
    {
        AudioManager.Play("sell");
    }

}
