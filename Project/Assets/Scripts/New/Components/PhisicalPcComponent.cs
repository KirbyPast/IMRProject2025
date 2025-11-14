using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable))]
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
        SpecialCreate();


        //TODO: Outline when hovered
        GetComponent<XRGrabInteractable>().hoverEntered.AddListener((args) =>
        {
            //Add outline when grabbed
        });

        GetComponent<XRGrabInteractable>().hoverExited.AddListener((args) =>
        {
            //Remove outline when released
        });
    }

    public virtual void SpecialCreate()
    {

    }
}
