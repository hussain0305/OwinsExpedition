using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Crossbow : BaseWeapon
{
    public GameObject CrossbowBolt;
    public float BoltSpeed;

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
    }

    void FireProjectile()
    {
        GameObject CurrentBolt = ObjectPooler.CentralObjectPool.SpawnFromPool(CrossbowBolt.name, transform.position, Arm.transform.rotation);
        CurrentBolt.GetComponent<PlayerAttackProjectile>().SetProjectileDamage((int)WeaponDamage.FetchCurrentElementValue(CurrentElement));
        CurrentBolt.GetComponent<PlayerAttackProjectile>().SetElementIndex(CurrentElement);
        CurrentBolt.GetComponent<Rigidbody2D>().velocity = transform.up * 10.0f;
    }
}
