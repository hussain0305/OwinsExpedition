using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Icykill : MonoBehaviour
{
    private AIIcehead Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIIcehead>();
    }

    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_SpawnProjectile()
    {
        Link.SpawnProjectile();
    }

    public void Proxy_ResetBools()
    {
        Link.ResetBools();
    }
}
