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
    public Button B_Complete, B_Sell;
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

        PrepareSellButton(@case.Completed);
        
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

        if(!CurrentCase.CheckCompleteness(out var caseInfo))
        {
            I_Err.gameObject.SetActive(true);
            T_Err.text = caseInfo;
            StartCoroutine(ActionAfterTime(() => {
                I_Err.gameObject.SetActive(false);
            }, 2));
            return;
        }
        foreach (var comp in AllChildrenComponents)
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

        CurrentCase.Completed = true;       

        var allComponentChildren = CurrentCase.transform.FindAllDeepChildren<PhisicalPcComponent>();
        foreach(var comp in allComponentChildren)
        {
            var colliders = comp.GetComponentsInChildren<Collider>();
            var obj = comp.gameObject;
            foreach (var c in colliders)
            {
                Destroy(c);
            }

            //Destroy(comp);
            obj.GetComponent<XRGrabInteractable>().enabled = false;
            obj.GetComponent<XRGeneralGrabTransformer>().enabled = false;
            obj.GetComponent<Rigidbody>().isKinematic = true;
                      
        }

        PrepareSellButton(true);
    }

    private IEnumerator ActionAfterTime(Action A, float time)
    {
        yield return new WaitForSecondsRealtime(time);
        A?.Invoke();
    }

    public void PrepareSellButton(bool state)
    {
        if (AllChildrenComponents.Count == 0 || CurrentCase == null)
            return;

        B_Sell.gameObject.SetActive(state);

        if (!state) return;

        B_Sell.GetComponentInChildren<TMP_Text>().text = "Complete Quest";

        B_Sell.onClick.RemoveAllListeners();
        B_Sell.onClick.AddListener(() =>
        {
            if (QuestManager.Instance.TrySubmitBuild(CurrentCase, out string failReason))
            {
                CurrentCase.gameObject.SetActive(false);
                gameObject.SetActive(false); // Close the UI tab
                Singleton.Orders.CreateSoldOrder(AllChildrenComponents);
            }
            else
            {
                I_Err.gameObject.SetActive(true);
                T_Err.text = failReason;
                StartCoroutine(ActionAfterTime(() => {
                    I_Err.gameObject.SetActive(false);
                }, 3f));
            }
        });
    }
}
