using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Maneotaur : MonoBehaviour
{
    private AIManeotaur Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIManeotaur>();
    }


    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_SlamShake()
    {
        Link.SlamShake();
    }

    public void Proxy_SpeedUp()
    {
        Link.SpeedUp();
    }

    public void Proxy_SlamSound()
    {
        Link.PlaySlamSound();
    }

    public void Proxy_FinishSlamAttack()
    {
        Link.FinishSlamAttack();
    }
}
