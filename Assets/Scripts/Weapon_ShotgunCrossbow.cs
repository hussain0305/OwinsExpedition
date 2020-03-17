using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_ShotgunCrossbow : BaseWeapon
{
    public GameObject CrossbowBolt;
    public float BoltSpeed;
    /*
    private float Cooldown = 0.5f;
    private float TimePassed;
    */
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
        FireProjectile();
    }

    void FireProjectile()
    {
        foreach(Transform Bolt in transform)
        {
            if (Bolt.name.Contains("Haze"))
            {
                break;
            }
            //GameObject CurrentBolt = Instantiate(CrossbowBolt, Bolt.transform.position, Bolt.transform.rotation);
            GameObject CurrentBolt =ObjectPooler.CentralObjectPool.SpawnFromPool(CrossbowBolt.name, Bolt.transform.position, Bolt.transform.rotation);
            CurrentBolt.GetComponent<PlayerAttackProjectile>().SetProjectileDamage((int)WeaponDamage.FetchCurrentElementValue(CurrentElement));
            CurrentBolt.GetComponent<PlayerAttackProjectile>().SetElementIndex(CurrentElement);
            CurrentBolt.GetComponent<Rigidbody2D>().velocity = CurrentBolt.transform.up * 10.0f;
        }
    }
}
