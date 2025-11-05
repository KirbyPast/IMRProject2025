using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MotherBoard : PcComponent
{
    public MotherBoard(string Name, string Description, string Manufacturer, string Model, float Price, List<Spec> Specs) : base(Name, Description, Manufacturer, Model, Price, Specs)
    {
    }
}
