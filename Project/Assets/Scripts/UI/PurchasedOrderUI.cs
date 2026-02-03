using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PurchasedOrderUI : MonoBehaviour
{
    private static readonly WaitForSeconds _waitForSeconds0_01 = new(0.01f);
    public PcComponent component;

    public TMP_Text T_Name, T_Price, T_Status;
    public Image I_Status;
    public Slider S_Progress;

    public List<string> StagesNames = new();
    public List<Color> StagesColors = new();

    public void Create(PcComponent component, Action onDelivered, float time = 10)
    {
        this.component = component;
        T_Name.text = component.Name;
        T_Price.text = "Total: $" + component.Price;
        StartDelivery(time, onDelivered);
    }

    public void StartDelivery(float time, Action onDelivered)
    {
        StartCoroutine(StartDeliveryCor(time, onDelivered));
    }


    private IEnumerator StartDeliveryCor(float time, Action onDelivered)
    {
        float curr = 0;
        int currStage = 0;
        S_Progress.maxValue = time;
        S_Progress.value = 0;

        T_Status.text = StagesNames[0];
        I_Status.color = StagesColors[0];

        while (curr < time)
        {
            yield return _waitForSeconds0_01;
            curr += 0.01f;
            S_Progress.value = curr;

            int stage = (int)(curr / (time / (StagesNames.Count - 1)));

            if (stage != currStage)
            {
                currStage = stage;
                T_Status.text = StagesNames[stage];
                I_Status.color = StagesColors[stage];

                if (currStage < 5)
                {
                    yield return new WaitForSeconds(UnityEngine.Random.Range(1, 5));
                }
            }
        }
        AudioManager.Play("DeliveryComplete");
        onDelivered.Invoke();
    }
}
