using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cpu : PcComponent
{

    public Cpu(string Name, string Description, float Price, string Manufacturer, string Model, List<Spec> specs) : base(Name, Description, Manufacturer, Model, Price, specs)
    {

    }
}
