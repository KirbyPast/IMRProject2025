using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public Target OriginalTarget;
    public int MaxCount = 10;

    private void Awake()
    {
        StartCoroutine(SpawnTargetsCor());
    }

    private IEnumerator SpawnTargetsCor()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(1);
            var cnt = FindObjectsOfType<Target>().Count();
            if (cnt > MaxCount)
            {
                continue;
            }
            var newTarget = Instantiate(OriginalTarget, transform);
            newTarget.gameObject.SetActive(true);
            var pos = Random.insideUnitCircle * 0.5f;
            newTarget.Spawn(new(pos.x, pos.y, 0), Random.Range(0, 5));
        }
    }
}
