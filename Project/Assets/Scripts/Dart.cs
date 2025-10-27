using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : MonoBehaviour
{
    public float Timeout = 5;
    public bool Fired;
    public void Fire()
    {
        StartCoroutine(TimeoutCor(Timeout));
        Fired = true;
    }

    private void Update()
    {
        if(Fired)
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 1));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Target>(out var t))
        {
            GameManager.HitTarget();
            Destroy(t.gameObject);
        }
    }

    private IEnumerator TimeoutCor(float f)
    {
        yield return new WaitForSecondsRealtime(f);
        Destroy(gameObject);
    }
}
