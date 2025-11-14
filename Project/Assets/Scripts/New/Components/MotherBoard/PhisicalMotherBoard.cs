using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PhisicalMotherBoard : PhisicalPcComponent
{
    public GameObject CpuHighlight;
    public List<(GameObject, bool)> RamHighlits = new();
    public bool CpuMounted = false;

    //TODO: Add system to keep track of added items and be able to remove them

    private void Start()
    {
        //??Maybe not remove XrGrabInteractible to be able to remove the component later
        Singleton.ItemGrabManager.OnItemDropped += (item) =>
        {
            if(!CpuMounted && item.GetComponent<PhisicalCpu>() != null && Vector3.Distance(item.transform.position, CpuHighlight.transform.position) < 0.1f)
            {
                Destroy(item.GetComponent<XRGrabInteractable>());

                Destroy(item.GetComponent<Rigidbody>());
                item.transform.parent = transform.GetChild(0);
                Tween.LocalPosition(item.transform, CpuHighlight.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                Tween.Rotation(item.transform, CpuHighlight.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                CpuHighlight.SetActive(false);
                CpuMounted = true;
            }

            if(RamHighlits.Exists(r => !r.Item2))
            {
                if (item.GetComponent<PhisicalRam>() != null)
                {
                    RamHighlits.ForEach(r => r.Item1.SetActive(false));

                    var min = 1f;
                    var closestRam = (GameObject)null;

                    foreach (var (ramHighlight, mounted) in RamHighlits.Where(r => !r.Item2))
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
                        Destroy(item.GetComponent<XRGrabInteractable>());

                        Destroy(item.GetComponent<Rigidbody>());
                        item.transform.parent = transform.GetChild(0);
                        Tween.LocalPosition(item.transform, closestRam.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, closestRam.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        RamHighlits[RamHighlits.FindIndex(r => r.Item1 == closestRam)] = (closestRam, true);
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
                var item = Singleton.ItemGrabManager.CurrentItems.Find(it => it.GetComponent<PhisicalCpu>() != null);
                if (item != null && Vector3.Distance(item.transform.position, CpuHighlight.transform.position) < 0.1f)
                {
                    CpuHighlight.SetActive(true);
                }
            }
            if (RamHighlits.Exists(r => !r.Item2))
            {
                var item = Singleton.ItemGrabManager.CurrentItems.Find(it => it.GetComponent<PhisicalRam>() != null);
                var min = 1f;
                var closestRam = (GameObject)null;
                if (item != null)
                {
                    foreach (var (ramHighlight, mounted) in RamHighlits.Where(r => !r.Item2))
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

        RamHighlits = transform.FindAllDeepChildren("RamHighlight").Select(t => (t.gameObject, false)).ToList();
        if(RamHighlits.Count == 0)
        {
            Debug.LogError("RamPlaceholders not found in MotherBoard");
        }
    }
}
