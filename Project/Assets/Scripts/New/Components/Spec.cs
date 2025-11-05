using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spec
{
    public string Name;
    public string Value;
    public Spec(string name, string value)
    {
        Name = name;
        Value = value;
    }

    public override string ToString()
    {
        return Name+ ": " + Value;
    }

}

