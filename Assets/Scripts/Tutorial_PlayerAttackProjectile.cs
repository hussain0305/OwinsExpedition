using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_PlayerAttackProjectile : MonoBehaviour
{

    public int ProjectileDamage;

    private int ElementIndex = 0;

    // Use this for initialization
    void Start()
    {

    }
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
            Tutuorial_BaseEnemy Victim = collision.gameObject.GetComponent<Tutuorial_BaseEnemy>();

            if (Victim)
            {
                Victim.CalculateDamage(ProjectileDamage, ElementIndex);
            }

            if (this.gameObject.tag != "Supermove")
            {
                Destroy(this.gameObject);
            }
            else
            {
                foreach(GameObject CurrentEnemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    CurrentEnemy.SetActive(false);
                }
            }
        }
    }
}
