using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PcComponentUI : MonoBehaviour
{
    private PcComponent thisComponent;
    public TMP_Text T_Name, T_Description, T_Price;
    public Button B_View, B_Buy;

    public void Create(Shop s, PcComponent component)
    {
        thisComponent = component;
        T_Name.text = component.Name;
        T_Description.text = component.Description;
        T_Price.text = component.Price.ToString();
        B_Buy.onClick.AddListener(() => s.BuyComponent(this, thisComponent));
        
    }
}
