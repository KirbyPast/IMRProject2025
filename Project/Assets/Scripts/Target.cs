using Pixelplacement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public void Spawn(Vector3 pos, int type)
    {
        transform.localPosition = pos;
        switch (type)
        {        
            case 1:
                Tween.LocalPosition(transform, transform.localPosition + new Vector3(0.5f, 0, 0), Random.Range(0.5f, 3), 0, loop: Tween.LoopType.PingPong);
                break;
            case 2:
                Tween.LocalPosition(transform, transform.localPosition + new Vector3(-0.5f, 0, 0), Random.Range(0.5f, 3), 0, loop: Tween.LoopType.PingPong);
                break;
            case 3:
                Tween.LocalPosition(transform, transform.localPosition + new Vector3(0, 0.5f, 0), Random.Range(0.5f, 3), 0, loop: Tween.LoopType.PingPong);
                break;
            case 4:
                Tween.LocalPosition(transform, transform.localPosition + new Vector3(0, -0.5f, 0), Random.Range(0.5f, 3), 0, loop: Tween.LoopType.PingPong);
                break;
            default:
                break;
        }
    }
}
