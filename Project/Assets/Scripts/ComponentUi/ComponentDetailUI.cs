using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComponentDetailUI : MonoBehaviour
{
    public TMP_Text T_TypeName;
    public ComponentDetailInfoUI OrgInfo;
    public List<ComponentDetailInfoUI> AllOrgInfos = new();
   

    public void Create(List<PhisicalPcComponent> components)
    {
        if (!AllSameChildType(components, out Type T))
            return;

        var name = T.Name;
        const string prefix = "Phisical";
        if (name.StartsWith(prefix))
            name = name[prefix.Length..];
        T_TypeName.text = name; 

        foreach(var cmp in components)
        {
            CreateComponentDetailInfo(cmp.thisComponent);
        }
    }


    public bool AllSameChildType(IReadOnlyList<PhisicalPcComponent> list, out Type T)
    {
        T = null;
        if (list == null || list.Count == 0) return false;

        var t = list[0].GetType();
        T = t;
        for (int i = 1; i < list.Count; i++)
            if (list[i] == null || list[i].GetType() != t)
                return false;

        return true;
    }

    public void CreateComponentDetailInfo(PcComponent pc)
    {
        var newInfo = Instantiate(OrgInfo, transform);
        string info = pc.Name + "\n\n" + pc.Description + "\n\n" + string.Join("\n-", pc.Specs);
        newInfo.Create(info);
        newInfo.gameObject.SetActive(true);
        AllOrgInfos.Add(newInfo);
    }
}
