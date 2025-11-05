using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ComponentUI : MonoBehaviour
{
    private PcComponent thisComponent;
    public TMP_Text T_Name, T_Description, T_Price;
    public void Create(PcComponent component)
    {
        thisComponent = component;
        T_Name.text = component.Name;
        T_Description.text = component.Description;
        T_Price.text = component.Price.ToString();
    }


}
