using UnityEngine;
using TMPro;
using System.Linq;

public class QuestDisplayUI : MonoBehaviour
{
    [Header("Current Quest UI")]
    public TextMeshProUGUI clientText;
    public TextMeshProUGUI cpuTrackerText;
    public TextMeshProUGUI gpuTrackerText;
    public TextMeshProUGUI fanTrackerText;
    public TextMeshProUGUI mbTrackerText;
    public TextMeshProUGUI ramTrackerText;
    public TextMeshProUGUI psuTrackerText;
    public TextMeshProUGUI rewardText;

    [Header("New Quest UI")]
    public NewQuestUI NewQuestUI;


    private void Start()
    {
        QuestManager.Instance.OnQuestChanged += UpdateActiveQuestUI;

    }

    void Update()
    {
        // Real-time tracking: Only run if a PC is on the bench
        UpdatePartTracking();
    }

    void UpdateActiveQuestUI(Quest q)
    {
        clientText.text = $"Client: {q.clientName}";
        rewardText.text = $"Reward: <color=#00FF00>${q.reward:F2}</color>";
        // Initial text for trackers
        cpuTrackerText.text = $"[ ] {q.requiredCpuName}";
        gpuTrackerText.text = $"[ ] {q.requiredGpuName}";
        mbTrackerText.text = $"[ ] {q.requiredMbName}";
        ramTrackerText.text = $"[ ] {q.requiredRamName}";
        psuTrackerText.text = $"[ ] {q.requiredPsuName}";
        fanTrackerText.text = q.requiresFans ? "[ ] Cooling Fans" : "Fans: Optional";

        NewQuestUI.UpdateClient(clientText.text);
        NewQuestUI.UpdateReward(rewardText.text);

        NewQuestUI.AddOrUpdateQuest(cpuTrackerText, q.requiredCpuName, false);
        NewQuestUI.AddOrUpdateQuest(gpuTrackerText, q.requiredGpuName, false);
        NewQuestUI.AddOrUpdateQuest(mbTrackerText, q.requiredMbName, false);
        NewQuestUI.AddOrUpdateQuest(ramTrackerText, q.requiredRamName, false);
        NewQuestUI.AddOrUpdateQuest(psuTrackerText, q.requiredPsuName, false);
        NewQuestUI.AddOrUpdateQuest(fanTrackerText, q.requiresFans ? "Cooling Fans" : "Fans: Optional", !q.requiresFans);
    }

    void UpdatePartTracking()
    {
        if (QuestManager.Instance.currentQuest == null) return;
        PhisicalCase activeCase = FindObjectOfType<PhisicalCase>();
        if (activeCase == null) return;

        var installedParts = activeCase.GetComponentsInChildren<PhisicalPcComponent>();
        var q = QuestManager.Instance.currentQuest;

        // Helper to update text color based on presence
        void UpdateStatus(TextMeshProUGUI label, string partName)
        {
            bool hasPart = installedParts.Any(p => p.thisComponent?.Name == partName);
            label.text = hasPart ? $"<color=green>[✔] {partName}</color>" : $"[ ] {partName}";

            NewQuestUI.AddOrUpdateQuest(label, partName, hasPart);
        }

        UpdateStatus(cpuTrackerText, q.requiredCpuName);
        UpdateStatus(gpuTrackerText, q.requiredGpuName);
        UpdateStatus(mbTrackerText, q.requiredMbName);
        UpdateStatus(ramTrackerText, q.requiredRamName);
        UpdateStatus(psuTrackerText, q.requiredPsuName);
        if (q.requiresFans)
        {
            bool hasFans = installedParts.Any(p => p is PhisicalCooler || p.thisComponent is Cooler);

            fanTrackerText.text = hasFans
                ? "<color=green>[✔] Cooling Fans</color>"
                : "[ ] Cooling Fans";
            NewQuestUI.AddOrUpdateQuest(fanTrackerText, "Cooling Fans", hasFans);
        }
        else
        {
            fanTrackerText.text = "Fans: Optional";
            NewQuestUI.AddOrUpdateQuest(fanTrackerText, "Fans: Optional", true);
        }
    }


}