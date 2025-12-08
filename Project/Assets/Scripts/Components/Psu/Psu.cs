using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Psu : PcComponent
{
    public Psu(string Id, string Name, string Description, string Manufacturer, string Model, float Price, List<Spec> Specs) : base(Id, Name, Description, Manufacturer, Model, Price, Specs)
    {
    }
}
