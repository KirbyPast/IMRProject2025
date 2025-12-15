using Pixelplacement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhisicalCase : PhisicalPcComponent, IAttachableTo
{
    [HideInInspector]
    public int buttonHeightOffset = 1;
    public GameObject validatorButtonPrefab;
    public GameObject MotherboardHighlight;
    private GameObject MotherboardProjection;
    [HideInInspector]
    public GameObject PsuHighlight;

    public List<Slot> FanSlots = new();

    [Header("Case")]
    public bool MotherboardMounted = false;
    public bool PsuMounted = false;

    private void Start()
    {
        SpawnValidatorButton();
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
            if (FanSlots.Exists(r => !r.isOccupied) && item.TryGetComponent(out PhisicalCooler cooler) && GetClosestHighlight(FanSlots, item.gameObject, 0.25f, out var closestFan))
            {
                print("Trying to mount Fan");
                FanSlots.ForEach(r => r.slotObject.SetActive(false));

                AttachComponent(cooler,
                    () => { FanSlots[FanSlots.FindIndex(r => r.slotObject == closestFan)] = (closestFan, false); },
                    () => {
                        Tween.LocalPosition(item.transform, closestFan.transform.localPosition, 0.5f, 0, Tween.EaseInOut);
                        Tween.Rotation(item.transform, closestFan.transform.rotation, 0.5f, 0, Tween.EaseInOut);
                        FanSlots[FanSlots.FindIndex(r => r.slotObject == closestFan)] = (closestFan, true);
                    }
                );
            }
        };
    }

    private void SpawnValidatorButton()
    {
        if (validatorButtonPrefab == null)
        {
            validatorButtonPrefab = Resources.Load<GameObject>("models/PC_Validator_UI");
        }

        Vector3 spawnPos = transform.position + new Vector3(0, buttonHeightOffset, 0);

        GameObject btnObj = Instantiate(validatorButtonPrefab, spawnPos, Quaternion.identity);

        btnObj.transform.SetParent(this.transform);

        CaseFloatingButton btnScript = btnObj.GetComponent<CaseFloatingButton>();
        if(btnScript != null)
        {
            btnScript.Initialize(this);
        }
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

        if (FanSlots.Exists(r => !r.isOccupied) && Singleton.ItemGrabManager.HasType<PhisicalCooler>(out var gpuItem))
        {
            ShowClosestHighlight(FanSlots, gpuItem.gameObject, 0.25f);
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

        FanSlots = transform.FindAllDeepChildren("FanHighlight").Select(t => new Slot(t.gameObject, false)).ToList();
        if (FanSlots.Count == 0)
        {
            Debug.LogError("FanHighlights not found in MotherBoard");
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

        if(!PsuMounted)
        {
            missingPart = "Power Supply Unit is missing from the case!";
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
        return false;

    }

}
