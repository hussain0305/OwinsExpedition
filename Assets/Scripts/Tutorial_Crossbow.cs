using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Crossbow : Tutorial_BaseWeapon
{
    public GameObject CrossbowBolt;
    public float BoltSpeed;

    // Use this for initialization
    new void Start()
    {
        base.Start();

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
        GameObject CurrentBolt = Instantiate(CrossbowBolt, transform.position, Arm.transform.rotation);
        CurrentBolt.GetComponent<Tutorial_PlayerAttackProjectile>().SetProjectileDamage((int)WeaponDamage.FetchCurrentElementValue(CurrentElement));
        CurrentBolt.GetComponent<Tutorial_PlayerAttackProjectile>().SetElementIndex(CurrentElement);
        CurrentBolt.GetComponent<Rigidbody2D>().velocity = transform.up * 10.0f;
    }
}
