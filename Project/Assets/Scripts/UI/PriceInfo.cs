using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PriceInfo : MonoBehaviour
{
    public float Value;
    public TMP_Text T_Value;
    public float Step = 25;

    public Button B_Up, B_Down;
    public Action<float> OnChange;

    private void Awake()
    {
        UpdateText();
        B_Up.onClick.AddListener(() =>
        {
            Value += Step;
            UpdateText();
            OnChange?.Invoke(Value);
        });
        B_Down.onClick.AddListener(() =>
        {
            Value -= Step;
            if(Value < 0) Value = 0;
            UpdateText();
            OnChange?.Invoke(Value);
        });
    }

    public void UpdateText()
    {
        T_Value.text = Value.ToString();
    }
}
