using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapedEffectSpawner : MonoBehaviour
{

    public void SetCurrentElement(int Elem)
    {
        foreach(Transform CurrentChild in transform)
        {
            CurrentChild.GetComponent<EffectExhaustPoint>().SetCurrentElement(Elem);
        }
    }
}
