using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_IBall : MonoBehaviour
{
    private AIEyeMonster Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIEyeMonster>();
        //transform.parent.GetComponentInChildren<SpriteShadow>().SetLargeShadowFactor(0.9f);
        transform.parent.GetComponentInChildren<SpriteShadow>().SetSmallShadowFactor(0.8f);
        //transform.parent.GetComponentInChildren<SpriteShadow>().SpeedUpShadowByFactor(3);
        //transform.parent.GetComponentInChildren<SpriteShadow>().StartFlyerShadowPulse();
    }

    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_BlindingOn()
    {
        Link.StartBlinding();
    }

    public void Proxy_BlindingOff()
    {
        Link.EndBlinding();
    }
}
