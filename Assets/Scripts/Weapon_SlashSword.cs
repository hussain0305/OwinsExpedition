using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_SlashSword : BaseWeapon
{

    public GameObject Damager;

    // Use this for initialization
    new void Awake()
    {
        base.Awake();
    }

    new void OnEnable()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    public override void SwipeInitiated(Vector2 SwipeStart)
    {
        SwipeStartAt = SwipeStart;
    }

    public override void SwipeEnded(Vector2 SwipeEnd)
    {
        SwipeEndAt = SwipeEnd;
        if (SwipeEndAt.x > SwipeStartAt.x)
        {
            AttackRight();
        }
        else
        {
            AttackLeft();
        }
    }

    void AttackRight()
    {
        GetComponent<SpriteRenderer>().flipX = true;

        //GameObject Dam = Instantiate(Damager, transform.position, transform.rotation);
        GameObject Dam = ObjectPooler.CentralObjectPool.SpawnFromPool(Damager.name, transform.position, transform.rotation);
        Dam.GetComponent<AngleDamager>().SetDamageAndElement((int)WeaponDamage.FetchCurrentElementValue(CurrentElement), CurrentElement);
    }

    void AttackLeft()
    {
        GetComponent<SpriteRenderer>().flipX = false;

        //GameObject Dam = Instantiate(Damager, transform.position, transform.rotation);
        GameObject Dam = ObjectPooler.CentralObjectPool.SpawnFromPool(Damager.name, transform.position, transform.rotation);
        Dam.GetComponent<AngleDamager>().SetDamageAndElement((int)WeaponDamage.FetchCurrentElementValue(CurrentElement), CurrentElement);
        Dam.transform.RotateAround(transform.position, transform.up, 180);
    }
}
