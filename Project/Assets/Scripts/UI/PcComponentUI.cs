using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PcComponentUI : MonoBehaviour
{
    public PcComponent thisComponent;
    public TMP_Text T_Name, T_Description, T_Price;
    public Button B_View, B_Buy;
    public Image I_Sprite;

    public void Create(Shop s, PcComponent component)
    {
        thisComponent = component;
        T_Name.text = component.Name;
        T_Description.text = component.Description;
        T_Price.text = component.Price.ToString();

        B_Buy.onClick.AddListener(() => { s.BuyComponent(this, thisComponent); });
        B_View.onClick.AddListener(() => { Singleton.ViewPcComponentUI.gameObject.SetActive(true); Singleton.ViewPcComponentUI.Create(this); });

        I_Sprite.sprite = Resources.Load<Sprite>($"images/{component.ModelId}");       
    }
}
