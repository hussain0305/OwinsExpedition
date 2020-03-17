using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Ghost : MonoBehaviour
{
    private AIGhost Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIGhost>();
        //transform.parent.GetComponentInChildren<SpriteShadow>().StartFlyerShadowPulse();
    }

    public void Proxy_MeleeDash()
    {
        Link.MeleeDash();
    }

    public void Proxy_FinishAttack()
    {
        Link.FinishAttack();
    }

    public void Proxy_StartDash()
    {
        Link.StartDash();
    }

    public void Proxy_SpawnProjectile()
    {
        Link.SpawnProjectile();
    }
}
