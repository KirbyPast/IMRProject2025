using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NewQuestUI : MonoBehaviour
{
    public TMP_Text T_Client, T_Reward;

    public Dictionary<TextMeshProUGUI, QuestUI> AllQuestsUI = new();
    public QuestUI OriginalQuestUI;

    public void UpdateClient(string cilient)
    {
        T_Client.text = cilient;
    }
    public void UpdateReward(string reward)
    {
        T_Reward.text = reward;
    }

    public void AddOrUpdateQuest(TextMeshProUGUI text, string info, bool finishState)
    {
        if(AllQuestsUI.TryGetValue(text, out var value))
        {
            value.UpdateQuestInfo(info, finishState);
        }
        else
        {
            var newQuestUI = Instantiate(OriginalQuestUI, OriginalQuestUI.transform.parent);
            AllQuestsUI.Add(text, newQuestUI);
            newQuestUI.gameObject.SetActive(true);
            newQuestUI.UpdateQuestInfo(info, finishState);
        }
    }
}
