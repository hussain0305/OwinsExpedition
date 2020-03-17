using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Rabbit : MonoBehaviour
{
    private AIRabbit Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIRabbit>();
    }
    
    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_Lunge()
    {
        Link.Lunge();
    }

    public void Proxy_ForwardSpeed()
    {
        Link.RefreshForwardSpeed();
    }
}
