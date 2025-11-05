using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Gpu : PcComponent
{
    public string Maker;    

    public Gpu(string Name, string Description, float Price, string Manufacturer, string Maker, string Model, List<Spec> Specs) : base(Name, Description, Manufacturer, Model, Price, Specs)
    {
        this.Maker = Maker;
    }
}
