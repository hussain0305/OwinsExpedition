using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleDamager : MonoBehaviour {

    public int DamagerDamage = 600;

    private int ElementNo = 0;

    List<GameObject> ActorsToDamage;
    
    // Use this for initialization
    void OnEnable () {
        Invoke("GoAway", 0.5f);
        ActorsToDamage = new List<GameObject>();
    }
    
    void GoAway()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    void DelayedAttackOnEnemy()
    {
        foreach(GameObject CurrentEnemy in ActorsToDamage)
        {
            if (CurrentEnemy)
            {
                if(CurrentEnemy.GetComponent<BaseEnemy>())
                    CurrentEnemy.GetComponent<BaseEnemy>().CalculateDamage(DamagerDamage, ElementNo);
                else
                {
                    //Destroy(CurrentEnemy.gameObject);
                    CurrentEnemy.gameObject.SetActive(false);
                }
            }
        }
        ActorsToDamage.Clear();
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void SetDamageAndElement(int NewDamage, int Element)
    {
        DamagerDamage = NewDamage;
        ElementNo = Element;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            ActorsToDamage.Add(collision.gameObject);
            Invoke("DelayedAttackOnEnemy", 0.2f);
        }
    }
}
