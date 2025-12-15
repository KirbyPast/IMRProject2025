using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhisicalCooler : PhisicalPcComponent
{
    public float FanSpeed = 1;
    public List<GameObject> Blades = new();
    public bool Power;
    public override void SpecialCreate()
    {
        Blades = transform.FindAllDeepChildren("BLADES").Select(t => t.gameObject).ToList();
    }

    public override void SpecialAttatch()
    {
        Power = true;
    }

    public override void SpecialDeAttatch()
    {
        Power = false;
    }

    private void Update()
    {
        if(Power)
        {
            SpinBlades();
        }
    }

    public void SpinBlades()
    {
        foreach(var blade in Blades)
        {
            blade.transform.Rotate(new Vector3(0, 1 * FanSpeed * Time.deltaTime * 360, 0));
        }
    }
}
