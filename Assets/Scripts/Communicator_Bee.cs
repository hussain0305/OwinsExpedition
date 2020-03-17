using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Bee : MonoBehaviour
{
    private AIBat Link;

    // Start is called before the first frame update
    void Start()
    {
        Link = GetComponentInParent<AIBat>();
        //transform.parent.GetComponentInChildren<SpriteShadow>().StartFlyerShadowPulse();
    }
    

    public void Proxy_StopSpitAttack()
    {
        Link.StopSpitAttack();
    }

    public void Proxy_SpawnProjectile()
    {
        Link.SpawnProjectile();
    }
}
