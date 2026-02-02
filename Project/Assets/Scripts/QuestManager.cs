using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestManager : MonoBehaviour
{
    [Header("Settings")]
    public float rewardMarkup = 4.0f;

    public static QuestManager Instance;
    public Quest currentQuest;
 

    public System.Action<Quest> OnQuestChanged;


    private void Awake() => Instance = this;
    private void Start() => GenerateNewQuest();

    public void GenerateNewQuest()
    {
        string[] names = { "CyberPunk_Pro", "BudgetBob", "RenderFarm_Inc", "TechLover88" };
        string client = names[Random.Range(0, names.Length)];

        List<string> reqIds = new List<string>();
        float totalCost = 0;

        T PickPart<T>(out string partName) where T : PcComponent
        {
            var pool = Storage.Components.OfType<T>().ToList();
            if (pool.Count > 0)
            {
                var picked = pool[Random.Range(0, pool.Count)];
                reqIds.Add(picked.ModelId);
                totalCost += picked.Price;
                partName = picked.Name;
                return picked;
            }
            partName = "Any " + typeof(T).Name;
            return null;
        }

        PickPart<Cpu>(out string cpu);
        PickPart<Gpu>(out string gpu);
        PickPart<Ram>(out string ram);
        PickPart<MotherBoard>(out string mb);
        PickPart<Psu>(out string psu);

        bool needsFans = Random.value > 0.5f;
        string desc = $"Build a PC with: {cpu}, {gpu}, {ram}, {mb}, and {psu}.";

        currentQuest = new Quest(client, desc, totalCost * rewardMarkup, reqIds, needsFans, cpu, gpu, ram, mb, psu);
        OnQuestChanged?.Invoke(currentQuest);
    }

    public bool TrySubmitBuild(PhisicalCase pcCase, out string failReason)
    {
        // 1. Check if the PC even boots (Basic hardware check)
        if (!pcCase.CheckCompleteness(out failReason))
        {
            return false;
        }

        // 2. Get all components currently attached to the case/motherboard
        var installedParts = pcCase.GetComponentsInChildren<PhisicalPcComponent>();

        // 3. Validate specific Model IDs
        foreach (var reqId in currentQuest.requiredModelIds)
        {
            bool hasPart = installedParts.Any(p => p.thisComponent != null && p.thisComponent.ModelId == reqId);
            if (!hasPart)
            {
                failReason = $"Missing required component ID: {reqId}";
                return false;
            }
        }

        // 4. Validate Fans (Coolers)
        if (currentQuest.requiresFans)
        {
            int fanCount = pcCase.FanSlots.Count(s => s.isOccupied);
            if (fanCount == 0)
            {
                failReason = "Client specifically asked for cooling fans!";
                return false;
            }
        }

        // Success!
        CompleteQuest();
        return true;
    }

    public void CompleteQuest()
    {

        Singleton.Shop.ChangeMoney(currentQuest.reward);
        GenerateNewQuest();
    }
}