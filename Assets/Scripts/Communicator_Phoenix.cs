using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Phoenix : MonoBehaviour
{
    private AIPhoenix Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIPhoenix>();
        transform.parent.GetComponentInChildren<SpriteShadow>().SetLargeShadowFactor(1.4f);
    }

    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_FieryAttackSound()
    {
        Link.PlayFieryAttackSound();
    }

    public void Proxy_SpawnProjectile()
    {
        Link.SpawnProjectile();
    }

    public void Proxy_StopRangedAttack()
    {
        Link.StopRangedAttack();
    }
}
