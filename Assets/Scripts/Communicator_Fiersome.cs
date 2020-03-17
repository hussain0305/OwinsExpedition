using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Fiersome : MonoBehaviour
{
    private AIFirehead Link;
    
    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIFirehead>();
    }

    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_PlayFireguyAttackSound()
    {
        Link.PlayFireguyAttackSound();
    }

    public void Proxy_SpawnProjectile()
    {
        Link.SpawnProjectile();
    }

    public void Proxy_StopAttack()
    {
        Link.StopRangedAttack();
    }

}
