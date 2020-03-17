using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Pikeman : MonoBehaviour
{
    private AIPikeman Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIPikeman>();
    }

    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_PikeSound()
    {
        Link.PikeSound();
    }

    public void Proxy_SpawnProjectile()
    {
        Link.SpawnProjectile();
    }
}
