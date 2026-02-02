using System.Collections.Generic;

[System.Serializable]
public class Quest
{
    public string clientName;
    public string description;
    public float reward;
    public List<string> requiredModelIds = new List<string>();
    public bool requiresFans;
    public string requiredCpuName;
    public string requiredGpuName;
    public string requiredRamName;
    public string requiredMbName;
    public string requiredPsuName;

    public Quest(string name, string desc, float money, List<string> modelIds, bool fans,
                     string cpu, string gpu, string ram, string mb, string psu)
    {
        clientName = name;
        description = desc;
        reward = money;
        requiredModelIds = modelIds;
        requiresFans = fans;
        requiredCpuName = cpu;
        requiredGpuName = gpu;
        requiredRamName = ram;
        requiredMbName = mb;
        requiredPsuName = psu;
    }
}