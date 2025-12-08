using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceInfo : MonoBehaviour
{
    public float Price;
    public TMP_Text T_Price;
    public float Step = 25;

    public Button B_Up, B_Down;
    public Action<float> OnChange;

    private void Awake()
    {
        UpdateText();
        B_Up.onClick.AddListener(() =>
        {
            Price += Step;
            UpdateText();
            OnChange?.Invoke(Price);
        });
        B_Down.onClick.AddListener(() =>
        {
            Price -= Step;
            if(Price < 0) Price = 0;
            UpdateText();
            OnChange?.Invoke(Price);
        });
    }

    public void UpdateText()
    {
        T_Price.text = Price.ToString();
    }
}
