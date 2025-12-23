using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailsTab : MonoBehaviour
{
    public ComponentDetailUI OrgDetails;
    public List<ComponentDetailUI> AllDetails = new();
    public float aboveTreshold = 2;

    private void Update()
    {
        transform.LookAt(Singleton.Player.transform);
    }

    public void Create(PhisicalCase @case)
    {
        ResetDetails();
        var allComponentChildren = @case.transform.FindAllDeepChildren<PhisicalPcComponent>();
        var grouped = FindAndGroupByType(allComponentChildren);

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
}
