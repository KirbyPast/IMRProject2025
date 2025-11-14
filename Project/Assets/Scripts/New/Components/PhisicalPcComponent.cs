using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhisicalPcComponent : MonoBehaviour
{
    private PcComponent thisComponent;
    public GameObject mesh;
    public Material material;
    public void Create(PcComponent pc)
    {
        thisComponent = pc;

        var prefab = Resources.Load<GameObject>($"models/{thisComponent.ModelId}");
        if (prefab == null)
            prefab = Resources.Load<GameObject>("models/empty");

        mesh = Instantiate(prefab, transform);

        material = Resources.Load<Material>($"materials/{thisComponent.ModelId}");
        if (material == null)
            material = Resources.Load<Material>("materials/empty");

        mesh.GetComponentInChildren<MeshRenderer>().material = material;
    }
}
