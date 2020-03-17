using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Elements
{
    public float Regular;
    public float Ice;
    public float Fire;
    public float Magical;

    Elements(float R, float I, float F, float M)
    {
        Regular = R;
        Ice = I;
        Fire = F;
        Magical = M;
    }

    public void setElementalValues(float R, float I, float F, float M)
    {
        Regular = R;
        Ice = I;
        Fire = F;
        Magical = M;
    }

    public float FetchCurrentElementValue(int CurrentElemental)
    {
        switch (CurrentElemental)
        {
            case 0:
                return Regular;
            case 1:
                return Ice;
            case 2:
                return Fire;
            case 3:
                return Magical;
            default:
                return Regular;
        }
    }

    public float getRegular()
    {
        return Regular;
    }

    public float getIce()
    {
        return Ice;
    }

    public float getFire()
    {
        return Fire;
    }

    public float getMagical()
    {
        return Magical;
    }
}


public class Elemental : MonoBehaviour {

}
