using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cpu : PcComponent
{
    public Cpu(string Id, string Name, string Description, float Price, string Manufacturer, string Model, List<Spec> specs, Vector3 offset = default)
        : base(Id, Name, Description, Manufacturer, Model, Price, specs)
    {
    }
}