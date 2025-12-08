using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Case : PcComponent
{
    public Case(string Id, string Name, string Description, string Manufacturer, string Model, float Price, List<Spec> Specs) : base(Id, Name, Description, Manufacturer, Model, Price, Specs)
    {
    }
}
