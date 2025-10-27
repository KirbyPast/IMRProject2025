using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DartManager : MonoBehaviour
{
    public Dart OriginalDart;
    public int MaxCount = 10;

    private void Awake()
    {
        StartCoroutine(SpawnCor());
    }

    private IEnumerator SpawnCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            var currentCnt = FindObjectsOfType<Dart>().Count();
            if (currentCnt > MaxCount)
            {
                continue;
            }
            var newDart = Instantiate(OriginalDart);
            newDart.gameObject.SetActive(true);
        }
    }
}
