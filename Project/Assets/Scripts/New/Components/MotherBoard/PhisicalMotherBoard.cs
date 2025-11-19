using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhisicalMotherBoard : PhisicalPcComponent
{
    public GameObject CpuHighlight;
    public List<(GameObject, bool)> RamSlots = new();
    public bool CpuMounted = false;

    private void Start()
    {
        Singleton.ItemGrabManager.OnItemDropped += (item) =>
        {
            print("Figuring if item dropped can be attached");
            if(!CpuMounted && item.GetComponent<PhisicalCpu>() != null && Vector3.Distance(item.transform.position, CpuHighlight.transform.position) < 0.1f)
            {
                print("Trying to mount CPU");
                AttachComponent(item.GetComponent<PhisicalPcComponent>(),
                            () => { CpuMounted = false; },
                            () => {
                                Tween.LocalPosition(item.transform, CpuHighlight.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                                Tween.Rotation(item.transform, CpuHighlight.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                                CpuHighlight.SetActive(false);
                                CpuMounted = true;
                            }
                        );
            }

            if(RamSlots.Exists(r => !r.Item2))
            {
                if (item.GetComponent<PhisicalRam>() != null)
                {
                    print("Trying to mount RAM");
                    RamSlots.ForEach(r => r.Item1.SetActive(false));

                    var min = 1f;
                    var closestRam = (GameObject)null;

                    foreach (var (ramHighlight, mounted) in RamSlots.Where(r => !r.Item2))
                    {
                        var dist = Vector3.Distance(item.transform.position, ramHighlight.transform.position);
                        ramHighlight.SetActive(false);
                        if (dist < min)
                        {
                            min = dist;
                            closestRam = ramHighlight;
                        }
                    }

                    if(closestRam != null && min <= 0.1f)
                    {
                        AttachComponent(item.GetComponent<PhisicalPcComponent>(),
                            () => { RamSlots[RamSlots.FindIndex(r => r.Item1 == closestRam)] = (closestRam, false); },
                            () => {
                                Tween.LocalPosition(item.transform, closestRam.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                                Tween.Rotation(item.transform, closestRam.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                                RamSlots[RamSlots.FindIndex(r => r.Item1 == closestRam)] = (closestRam, true);
                            }
                        );
                        
                    }                   
                }
            }
        };
    }

    private void Update()
    {
        if (Singleton.ItemGrabManager.CurrentItems.Count != 0)
        {
            if(!CpuMounted)
            {
                CpuHighlight.SetActive(false);
                var item = Singleton.ItemGrabManager.CurrentItems.Find(it => it.GetComponent<PhisicalCpu>() != null);
                if (item != null && Vector3.Distance(item.transform.position, CpuHighlight.transform.position) < 0.1f)
                {
                    CpuHighlight.SetActive(true);
                }
            }
            if (RamSlots.Exists(r => !r.Item2))
            {
                var item = Singleton.ItemGrabManager.CurrentItems.Find(it => it.GetComponent<PhisicalRam>() != null);
                var min = 1f;
                var closestRam = (GameObject)null;
                if (item != null)
                {
                    foreach (var (ramHighlight, mounted) in RamSlots.Where(r => !r.Item2))
                    {
                        var dist = Vector3.Distance(item.transform.position, ramHighlight.transform.position);
                        ramHighlight.SetActive(false);
                        if (dist < min)
                        {
                            min = dist;
                            closestRam = ramHighlight;
                        }
                    }
                    if (closestRam != null && min < 0.1f)
                    {
                        closestRam.SetActive(true);
                    }
                }
            }
        }
    }

    public override void SpecialCreate()
    {
        CpuHighlight = transform.FindDeepChild("CpuHighlight").gameObject;
        if (CpuHighlight == null)
        {
            Debug.LogError("CpuPlaceholder not found in MotherBoard");
        }

        RamSlots = transform.FindAllDeepChildren("RamHighlight").Select(t => (t.gameObject, false)).ToList();
        if(RamSlots.Count == 0)
        {
            Debug.LogError("RamPlaceholders not found in MotherBoard");
        }
    }

    public void AttachComponent(PhisicalPcComponent pc, Action OnDeAttach, Action SpecialBeh)
    {
        print("Mounting component to this motherboard.");
        pc.Attach(this);
        pc.OnDeAttach += () => { OnDeAttach?.Invoke(); };
        pc.gameObject.transform.SetParent(transform.GetChild(0), true);
        SpecialBeh?.Invoke();
    }
}
