using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonSludge : MonoBehaviour {

    private bool AttackedPlayer;
    
    // Use this for initialization
	void Start () {
        AttackedPlayer = false;
        BubblesOff();
    }
	
    void BubblesOff()
    {
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }
    public void BubblesOn()
    {
        this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
    }

    public bool ShouldDamagePlayer()
    {
        if (!AttackedPlayer)
        {
            AttackedPlayer = true;
            return true;
        }
        else
        {
            return false;
        }

    }
    
}
