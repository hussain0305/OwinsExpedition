using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_PoisonPool : MonoBehaviour {

    public int PoisonBombDamage;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BaseEnemy>())
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakePoisonDamage(PoisonBombDamage);
        }
    }
}
