using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComponentDetailInfoUI : MonoBehaviour
{
    public TMP_Text T_Info;
    public string info;

    public void Create(string info)
    {
        this.info = info;
        T_Info.text = info;
    }
}
