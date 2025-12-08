using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PcComponent
{
    public string ModelId;
    public string Name;
    public string Description;
    public string Manufacturer;
    public string Model;
    public float Price;
    public List<Spec> Specs = new();

    public PcComponent(string Id, string Name, string Description, string Manufacturer, string Model, float Price, List<Spec> Specs)
    {
        this.ModelId = Id;
        this.Name = Name;
        this.Manufacturer = Manufacturer;
        this.Price = Price;
        this.Model = Model;
        this.Specs = Specs;
        this.Description = Description;
    }
}
