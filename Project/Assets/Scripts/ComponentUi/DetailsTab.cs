using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class DetailsTab : MonoBehaviour
{
    public VerticalLayoutGroup DetailsLayout;
    public ComponentDetailUI OrgDetails;
    public List<ComponentDetailUI> AllDetails = new();
    public List<PhisicalPcComponent> AllChildrenComponents = new();
    public PhisicalCase CurrentCase;
    public Button B_Complete;
    public Image I_Err;
    public TMP_Text T_Err;
    public float aboveTreshold = 2;

    private void Awake()
    {
        B_Complete.onClick.AddListener(() => {
            VerifyAll();
        });
    }

    private void Update()
    {
        transform.LookAt(Singleton.Player.transform);
    }

    public void Create(PhisicalCase @case)
    {
        ResetDetails();
        CurrentCase = @case;

        var allComponentChildren = @case.transform.FindAllDeepChildren<PhisicalPcComponent>();
        var grouped = FindAndGroupByType(allComponentChildren);

        AllChildrenComponents = allComponentChildren;
        AllChildrenComponents.Add(@case);

        CreateDetail(new() { @case });
        foreach(var pair in  grouped)
        {
            CreateDetail(pair.Value);
        }


        StartCoroutine(ActionAfterTime(() => {
            DetailsLayout.enabled = false;
            StartCoroutine(ActionAfterTime(() => { 
                DetailsLayout.enabled = true; 
            }, 0.1f));
        }, 0.1f));
        
    }

    public void CreateDetail(List<PhisicalPcComponent> ppc)
    {
        var newDetail = Instantiate(OrgDetails, OrgDetails.transform.parent);
        AllDetails.Add(newDetail);
        newDetail.gameObject.SetActive(true);
        newDetail.Create(ppc);
    }


    public static Dictionary<Type, List<T>> FindAndGroupByType<T>(List<T> list)
    {
        var result = new Dictionary<Type, List<T>>();

        foreach (var item in list)
        {
            var type = item.GetType();
            if (!result.ContainsKey(type))
                result[type] = new();

            result[type].Add(item);
        }

        return result;
    }

    public void ResetDetails()
    {
        foreach(var item in AllDetails)
        {
            Destroy(item.gameObject);
        }
        AllDetails.Clear();
    }

    public void VerifyAll()
    {
        foreach(var comp in AllChildrenComponents)
        {
            if(!comp.CheckCompleteness(out var info))
            {
                I_Err.gameObject.SetActive(true);
                T_Err.text = info;

                StartCoroutine(ActionAfterTime(() => {
                    I_Err.gameObject.SetActive(false);
                }, 2));
                return;
            }
        }

        FinishBuild();
    }

    public void FinishBuild()
    {
        if (CurrentCase == null)
            return;

        Singleton.Orders.CreateSoldOrder(AllChildrenComponents);
        Singleton.Shop.ChangeMoney(AllChildrenComponents.Sum(c => c.thisComponent.Price));

        var allComponentChildren = CurrentCase.transform.FindAllDeepChildren<PhisicalPcComponent>();
        foreach(var comp in allComponentChildren)
        {
            var colliders = comp.GetComponentsInChildren<Collider>();
            var obj = comp.gameObject;
            foreach (var c in colliders)
            {
                Destroy(c);
            }

            Destroy(comp);
            Destroy(obj.GetComponent<XRGrabInteractable>());
            Destroy(obj.GetComponent<XRGeneralGrabTransformer>());
            Destroy(obj.GetComponent<Rigidbody>());
                      
        }
    }

    private IEnumerator ActionAfterTime(Action A, float time)
    {
        yield return new WaitForSeconds(time);
        A?.Invoke();
    }
}
