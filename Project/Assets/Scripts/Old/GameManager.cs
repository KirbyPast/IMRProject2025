using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int Score;
    public TMP_Text T_Score;

    void Start()
    {
        if(Instance == null)
            Instance = this;
    }

    public static void HitTarget()
    {
        Instance.T_Score.text = "Score: " + Instance.Score;
        Instance.Score++;
    }
}
