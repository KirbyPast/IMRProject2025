using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;
using VHACD.Unity;

[RequireComponent(typeof(XRGrabInteractable), typeof(Rigidbody))]
public class PhisicalPcComponent : MonoBehaviour
{
    private PcComponent thisComponent;

    [Header("Base component")] 
    public GameObject mesh;
    public Material material;
    public XRGrabInteractable Interactible;
    public Rigidbody Rigidbody;

    public bool Attached = false;
    public PhisicalPcComponent AttachedTo;
    public Action OnDeAttach;

    public List<Collider> Colliders = new();

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

        Colliders = transform.GetComponentsInChildren<Collider>().ToList();
        SpecialCreate();
    }

    public static void IgnoreColliders(PhisicalPcComponent p1, PhisicalPcComponent p2, bool state)
    {
        foreach(var cl1 in p1.Colliders)
        {
            foreach(var cl2 in p2.Colliders)
            {
                Physics.IgnoreCollision(cl1, cl2, state);
            }
        }
    }

    public void Attach(PhisicalPcComponent to)
    {
        print("Attaching component to " + to.name);
        Rigidbody.isKinematic = true;
        IgnoreColliders(this, to, true);
        AttachedTo = to;
        Attached = true;
    }

    public void DeAttach()
    {
        print("Deattaching component from " + AttachedTo.name);
        transform.SetParent(null);
        Rigidbody.isKinematic = false;


        IgnoreColliders(this, AttachedTo, false);

        AttachedTo = null;
        Attached = false;

        OnDeAttach?.Invoke();
        OnDeAttach = null;
    }

    public virtual void SpecialCreate()
    {

    }

    public virtual bool CheckCompleteness(out string missingPart)
    {
        missingPart = string.Empty;
        return true; 
    }
}

public interface IAttachableTo
{
    public void AttachComponent(PhisicalPcComponent pc, Action OnDeAttach, Action SpecialBeh);
}
