using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.UI.Outline))]
public class QuestUI : MonoBehaviour
{
    public TMP_Text T_Info;
    public bool finishState;
    

    public void UpdateQuestInfo(string info, bool finishState)
    {
        T_Info.text = info;
        this.finishState = finishState;
        GetComponent<UnityEngine.UI.Outline>().effectColor = finishState ? Color.green : Color.red;
    }
}
