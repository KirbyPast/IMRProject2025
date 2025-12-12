using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Paginator : MonoBehaviour
{
    public int Value;
    public TMP_Text T_Value;
    public int MaxOnPage, CurrentOnPage;
    public Button B_Left, B_Right;
    public Action<int> OnChange;

    private void Awake()
    {
        B_Left.onClick.AddListener(() =>
        {
            Value--;
            T_Value.text = Value.ToString();
            if(Value < 0)
                Value = 0;
            OnChange?.Invoke(Value);
            UpdateButtons();

        });

        B_Right.onClick.AddListener(() =>
        {
            if (CurrentOnPage < MaxOnPage)
                return;
            print("RIGHJT");

            T_Value.text = Value.ToString();
            Value++;
            OnChange?.Invoke(Value);
            UpdateButtons();
        });
    }


    public void UpdateCounters(int current, int max)
    {
        CurrentOnPage = current;
        MaxOnPage = max;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        B_Left.interactable = Value > 0;

        B_Right.interactable = CurrentOnPage >= MaxOnPage;
    }
}
