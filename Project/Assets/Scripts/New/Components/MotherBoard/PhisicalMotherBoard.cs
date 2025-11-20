using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhisicalMotherBoard : PhisicalPcComponent, IAttachableTo
{
    [HideInInspector]
    public GameObject CpuHighlight;

    [Header("Motherboard")] 
    public bool CpuMounted = false;
    public List<Slot> RamSlots = new();
    public List<Slot> GpuSlots = new();
    

    private void Start()
    {
        Singleton.ItemGrabManager.OnItemDropped += (item) =>
        {
            print("Figuring if item dropped can be attached");
            if(!CpuMounted && item.TryGetComponent(out PhisicalCpu cpu) && Vector3.Distance(item.transform.position, CpuHighlight.transform.position) < 0.1f)
            {
                print("Trying to mount CPU");
                AttachComponent(cpu,
                    () => { CpuMounted = false; },
                    () => {
                        Tween.LocalPosition(item.transform, CpuHighlight.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, CpuHighlight.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        CpuHighlight.SetActive(false);
                        CpuMounted = true;
                    }
                );
            }

            if(RamSlots.Exists(r => !r.isOccupied) && item.TryGetComponent(out PhisicalRam ram) && GetClosestHighlight(RamSlots, item.gameObject, 0.1f, out var closestRam))
            {
                print("Trying to mount RAM");
                RamSlots.ForEach(r => r.slotObject.SetActive(false));

                AttachComponent(ram,
                    () => { RamSlots[RamSlots.FindIndex(r => r.slotObject == closestRam)] = (closestRam, false); },
                    () => {
                        Tween.LocalPosition(item.transform, closestRam.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, closestRam.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        RamSlots[RamSlots.FindIndex(r => r.slotObject == closestRam)] = (closestRam, true);
                    }
                );                                                         
            }

            if (GpuSlots.Exists(r => !r.isOccupied) && item.TryGetComponent(out PhisicalGpu gpu) && GetClosestHighlight(GpuSlots, item.gameObject, 0.1f, out var closestGpu))
            {
                print("Trying to mount GPU");
                GpuSlots.ForEach(r => r.slotObject.SetActive(false));

                AttachComponent(gpu,
                    () => { GpuSlots[GpuSlots.FindIndex(r => r.slotObject == closestGpu)] = (closestGpu, false); },
                    () => {
                        Tween.LocalPosition(item.transform, closestGpu.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, closestGpu.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        GpuSlots[GpuSlots.FindIndex(r => r.slotObject == closestGpu)] = (closestGpu, true);
                    }
                );
            }
        };
    }

    private void Update()
    {
        if (Singleton.ItemGrabManager.CurrentItems.Count != 0)
        {
            if(!CpuMounted && Singleton.ItemGrabManager.HasType<PhisicalCpu>(out var cpuItem))
            {
                CpuHighlight.SetActive(Vector3.Distance(cpuItem.transform.position, CpuHighlight.transform.position) < 0.1f);
            }
            if (RamSlots.Exists(r => !r.isOccupied) && Singleton.ItemGrabManager.HasType<PhisicalRam>(out var ramItem))
            {
                ShowClosestHighlight(RamSlots, ramItem.gameObject, 0.1f);
            }
            if (GpuSlots.Exists(r => !r.isOccupied) && Singleton.ItemGrabManager.HasType<PhisicalGpu>(out var gpuItem))
            {
                ShowClosestHighlight(GpuSlots, gpuItem.gameObject, 0.1f);
            }
        }
    }

    public override void SpecialCreate()
    {
        CpuHighlight = transform.FindDeepChild("CpuHighlight").gameObject;
        if (CpuHighlight == null)
        {
            Debug.LogError("CpuHighlight not found in MotherBoard");
        }

        RamSlots = transform.FindAllDeepChildren("RamHighlight").Select(t => new Slot(t.gameObject, false)).ToList();
        if(RamSlots.Count == 0)
        {
            Debug.LogError("RamHighlights not found in MotherBoard");
        }

        GpuSlots = transform.FindAllDeepChildren("GpuHighlight").Select(t => new Slot(t.gameObject, false)).ToList();
        if (GpuSlots.Count == 0)
        {
            Debug.LogError("GpuHighlights not found in MotherBoard");
        }
    }

    public void ShowClosestHighlight(List<Slot> highlights, GameObject item, float treshold)
    {
        var min = Mathf.Infinity;
        var closestSlot = (GameObject)null;

        foreach (var (highlight, occupied) in highlights.Where(r => !r.isOccupied))
        {
            var dist = Vector3.Distance(item.transform.position, highlight.transform.position);
            highlight.SetActive(false);
            if (dist < min)
            {
                min = dist;
                closestSlot = highlight;
            }
        }

        if (min < treshold)
            closestSlot.SetActive(true);
        
    }

    public bool GetClosestHighlight(List<Slot> highlights, GameObject item, float treshold, out GameObject closestSlot)
    {
        var min = Mathf.Infinity;
        closestSlot = null;
        
        foreach (var (highlight, occupied) in highlights.Where(r => !r.isOccupied))
        {
            var dist = Vector3.Distance(item.transform.position, highlight.transform.position);
            highlight.SetActive(false);
            if (dist < min)
            {
                min = dist;
                closestSlot = highlight;
            }
        }

        if (min < treshold)        
            return true;

        closestSlot = null;
        return true;

    }

    public void AttachComponent(PhisicalPcComponent pc, Action OnDeAttach, Action SpecialBeh)
    {
        print("Mounting component to this component.");
        pc.Attach(this);
        pc.OnDeAttach += () => { OnDeAttach?.Invoke(); };
        pc.gameObject.transform.SetParent(transform.GetChild(0), true);
        SpecialBeh?.Invoke();
    }
}

[Serializable]
public struct Slot
{
    [HideInInspector]
    public GameObject slotObject;
    public bool isOccupied;

    public Slot(GameObject slotObject, bool isOccupied)
    {
        this.slotObject = slotObject;
        this.isOccupied = isOccupied;
    }

    public void Deconstruct(out GameObject slotObject, out bool isOccupied)
    {
        slotObject = this.slotObject;
        isOccupied = this.isOccupied;
    }

    public static implicit operator (GameObject, bool)(Slot r) => (r.slotObject, r.isOccupied);
    public static implicit operator Slot((GameObject slotObject, bool Item2) tuple) => new(tuple.slotObject, tuple.Item2);
}
