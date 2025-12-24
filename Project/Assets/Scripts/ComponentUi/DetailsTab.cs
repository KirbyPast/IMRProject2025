using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DetailsTab : MonoBehaviour
{
    public ComponentDetailUI OrgDetails;
    public List<ComponentDetailUI> AllDetails = new();
    public List<PhisicalPcComponent> AllChildrenComponents = new();
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

        var allComponentChildren = @case.transform.FindAllDeepChildren<PhisicalPcComponent>();
        var grouped = FindAndGroupByType(allComponentChildren);

        AllChildrenComponents = allComponentChildren;
        AllChildrenComponents.Add(@case);

        CreateDetail(new() { @case });
        foreach(var pair in  grouped)
        {
            CreateDetail(pair.Value);
        }
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

                StartCoroutine(ActionAfterTIme(() => {
                    I_Err.gameObject.SetActive(false);
                }, 2));
                return;
            }
        }

        FinishBuild();
    }

    public void FinishBuild()
    {
        print("Finishing build!");
    }

    private IEnumerator ActionAfterTIme(Action A, float time)
    {
        yield return new WaitForSeconds(time);
        A?.Invoke();
    }
}
