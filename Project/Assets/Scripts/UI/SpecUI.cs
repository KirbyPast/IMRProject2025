using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpecUI : MonoBehaviour
{
    public Spec thisSpec;

    public TMP_Text T_Name, T_Value;

    public void Create(Spec spec)
    {
        thisSpec = spec; 
        T_Name.text = spec.Name;
        T_Value.text = spec.Value;
    }
}
