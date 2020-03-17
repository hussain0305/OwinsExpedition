using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communicator_Goblin : MonoBehaviour
{
    private AIGoblin Link;

    // Start is called before the first frame update
    void Awake()
    {
        Link = GetComponentInParent<AIGoblin>();

    }

    public void ShadowSizeUp()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().EnlargeShadow();
    }

    public void ShadowSizeDown()
    {
        transform.parent.GetComponentInChildren<SpriteShadow>().DiminishShadow();
    }

    public void Proxy_MinionRise()
    {
        Link.TriggerMinionRise();
    }
}
