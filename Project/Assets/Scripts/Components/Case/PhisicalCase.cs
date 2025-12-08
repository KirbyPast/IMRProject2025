using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhisicalCase : PhisicalPcComponent, IAttachableTo
{
    [HideInInspector]
    public GameObject MotherboardHighlight;
    private GameObject MotherboardProjection;
    [HideInInspector]
    public GameObject PsuHighlight;

    [Header("Case")]
    public bool MotherboardMounted = false;
    public bool PsuMounted = false;

    private void Start()
    {
        MotherboardProjection = Instantiate(MotherboardHighlight, MotherboardHighlight.transform.parent);
        MotherboardProjection.name = "MotherboardProjection";
        Singleton.ItemGrabManager.OnItemDropped += (item) =>
        {
            if(!MotherboardMounted && item.TryGetComponent(out PhisicalMotherBoard mtr) && Vector3.Distance(item.transform.position, MotherboardHighlight.transform.position) < 0.2f)
            {
                print("Trying to mount Motherboard");
                AttachComponent(mtr,
                    () => { MotherboardMounted = false; },
                    () => {
                        Tween.LocalPosition(item.transform, MotherboardProjection.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, MotherboardProjection.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        MotherboardProjection.SetActive(false);
                        MotherboardMounted = true;
                    }
                );
            }
            if (!PsuMounted && item.TryGetComponent(out PhisicalPsu psu) && Vector3.Distance(item.transform.position, PsuHighlight.transform.position) < 0.5f)
            {
                print("Trying to mount Motherboard");
                AttachComponent(psu,
                    () => { PsuMounted = false; },
                    () => {
                        Tween.LocalPosition(item.transform, PsuHighlight.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, PsuHighlight.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        PsuHighlight.SetActive(false);
                        PsuMounted = true;
                    }
                );
            }
        };
    }


    private void Update()
    {
        if (Singleton.ItemGrabManager.CurrentItems.Count != 0)
        {
            if (!MotherboardMounted && Singleton.ItemGrabManager.HasType<PhisicalMotherBoard>(out var mtrItem))
            {
                var actualBoard = mtrItem.transform.GetChild(0);

                bool show = Vector3.Distance(mtrItem.transform.position, MotherboardHighlight.transform.position) < 0.2f;
                MotherboardProjection.SetActive(show);

                if (show && TransformExtensions.TryGetTrueXYSize(actualBoard, out float trueW, out float trueH))
                {
                    TransformExtensions.MatchProjectionXY_ByTrueSize(MotherboardProjection.transform, MotherboardHighlight.transform, trueW, trueH);
                    TransformExtensions.SnapXYOnPlane(MotherboardProjection.transform, MotherboardHighlight.transform, actualBoard);

                    var difZ = (MotherboardHighlight.transform.localScale.x - MotherboardProjection.transform.localScale.x) / 2;
                    var difY = (MotherboardHighlight.transform.localScale.y - MotherboardProjection.transform.localScale.y) / 2;
                    MotherboardProjection.transform.localPosition = new Vector3(
                        MotherboardProjection.transform.localPosition.x,
                        Mathf.Clamp(
                            MotherboardProjection.transform.localPosition.y,
                            MotherboardHighlight.transform.localPosition.y - difY,
                            MotherboardHighlight.transform.localPosition.y + difY
                            ),
                        Mathf.Clamp(
                            MotherboardProjection.transform.localPosition.z,
                            MotherboardHighlight.transform.localPosition.z - difZ,
                            MotherboardHighlight.transform.localPosition.z + difZ
                            )
                        );
                }
            }
            if(!PsuMounted && Singleton.ItemGrabManager.HasType<PhisicalPsu>(out var psuItem))
            {
                PsuHighlight.SetActive(Vector3.Distance(psuItem.transform.position, PsuHighlight.transform.position) < 0.5f);
            }
        }
    }

    public override void SpecialCreate()
    {
        MotherboardHighlight = transform.FindDeepChild("MotherboardHighlight").gameObject;
        if (MotherboardHighlight == null)
        {
            Debug.LogError("MotherboardHighlight not found in Case");
        }

        PsuHighlight = transform.FindDeepChild("PsuHighlight").gameObject;
        if (PsuHighlight == null)
        {
            Debug.LogError("MotherboardHighlight not found in Case");
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


    public override bool CheckCompleteness(out string missingPart)
    {
        if (!MotherboardMounted)
        {
            missingPart = "Motherboard is missing from the case!";
            return false;
        }

        var attachedMotherboard = transform.GetChild(0).GetComponentInChildren<PhisicalMotherBoard>();

        if (attachedMotherboard == null)
        {
            missingPart = "Motherboard logic not found!";
            return false;
        }

        return attachedMotherboard.CheckCompleteness(out missingPart);
    }

}
