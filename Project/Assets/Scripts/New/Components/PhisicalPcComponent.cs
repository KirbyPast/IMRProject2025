using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class PhisicalPcComponent : MonoBehaviour
{
    private PcComponent thisComponent;
    public GameObject mesh;
    public Material material;
    public XRGrabInteractable Interactible;
    public Rigidbody Rigidbody;

    public bool Attached = false;
    public PhisicalPcComponent AttachedTo;
    public Action OnDeAttach;
    Transform lastParent;

    public void Create(PcComponent pc)
    {
        lastParent = transform.parent;
        thisComponent = pc;

        var prefab = Resources.Load<GameObject>($"models/{thisComponent.ModelId}");
        if (prefab == null)
            prefab = Resources.Load<GameObject>("models/empty");

        mesh = Instantiate(prefab, transform);

        material = Resources.Load<Material>($"materials/{thisComponent.ModelId}");
        if (material == null)
            material = Resources.Load<Material>("materials/empty");

        mesh.GetComponentInChildren<MeshRenderer>().material = material;

        Interactible = GetComponent<XRGrabInteractable>();
        Rigidbody = GetComponent<Rigidbody>();

        Interactible.selectEntered.AddListener((a) =>
        {
            if(Attached)
                DeAttach();
        });
        Interactible.selectExited.AddListener((a) =>
        {
            if (!Attached)
            {
                transform.SetParent(null);
                Rigidbody.isKinematic = false;
            }
        });

        SpecialCreate();
    }

    public void Attach(PhisicalPcComponent to)
    {
        print("Attaching component to " + to.name);
        Rigidbody.isKinematic = true;
        Physics.IgnoreCollision(GetComponentInChildren<Collider>(), to.GetComponentInChildren<Collider>(), true);
        AttachedTo = to;
        Attached = true;
    }

    public void DeAttach()
    {
        print("Deattaching component from " + AttachedTo.name);
        transform.SetParent(null);
        Rigidbody.isKinematic = false;
           
        
        Physics.IgnoreCollision(GetComponentInChildren<Collider>(), AttachedTo.GetComponentInChildren<Collider>(), false);

        AttachedTo = null;
        Attached = false;

        OnDeAttach?.Invoke();
        OnDeAttach = null;
    }

    public virtual void SpecialCreate()
    {

    }

    void OnTransformParentChanged()
    {
        Debug.Log($"[ParentChangeProbe] {name} parent -> {(transform.parent ? transform.parent.name : "null")}\n{new System.Diagnostics.StackTrace()}");
        lastParent = transform.parent;
    }
}
