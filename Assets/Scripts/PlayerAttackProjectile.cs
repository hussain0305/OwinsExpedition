using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackProjectile : MonoBehaviour {

    public int ProjectileDamage;

    private int ElementIndex = 0;

    public void SetProjectileDamage(int Damage)
    {
        ProjectileDamage = Damage;
    }

    public int GetProjectileDamage()
    {
        return ProjectileDamage;
    }

    public void SetElementIndex(int Index)
    {
        ElementIndex = Index;
        if (gameObject.GetComponent<ElementColoringScript>())
        {
            gameObject.GetComponent<ElementColoringScript>().SetColorByElement(Index);
        }
    }

    public int GetElementIndex()
    {
        return ElementIndex;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            BaseEnemy Victim = collision.gameObject.GetComponent<BaseEnemy>();

            if (Victim)
            {
                Victim.CalculateDamage(ProjectileDamage, ElementIndex);
            }

            if(this.gameObject.tag != "Supermove")
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
