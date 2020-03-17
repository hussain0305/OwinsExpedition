using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Fireball : MonoBehaviour {

    // Use this for initialization
    public GameObject Crater;

    private int Damage = 0;
    private Vector3 OriginalScale;
    private Weapon_Firestorm ParentStorm;
    private Rigidbody2D Body;
    private CircleCollider2D CollisionArea;

    void Awake () {
        Body = GetComponent<Rigidbody2D>();
        Damage = 0;
        CollisionArea = GetComponent<CircleCollider2D>();
        CollisionArea.enabled = false;
        OriginalScale = transform.localScale;
    }

    private void OnEnable()
    {
        Damage = 0;
        CollisionArea.enabled = false;
        transform.localScale = OriginalScale;
    }

    public void SetParentStorm(Weapon_Firestorm Parent)
    {
        ParentStorm = Parent;
    }

    public void CallFireballSpawn()
    {
        if (ParentStorm)
        {
            ParentStorm.RainFirestorm();
        }
    }

    public void FormCrater()
    {
        //Instantiate(Crater, transform.position - new Vector3(0, 0.4f, 0), transform.rotation);
        ObjectPooler.CentralObjectPool.SpawnFromPool(Crater.name, transform.position - new Vector3(0, 0.4f, 0), transform.rotation);
    }

    //WORLD MOVEMENT BELOW

    public void StartWorldMovement()
    {
        transform.localScale = 2 * (transform.localScale);
        Body.velocity = new Vector2(0.0f, -2.25f);
    }

    public void DeleteFireball()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    public void FireBombExploded()
    {
        Damage = 20;
        CollisionArea.enabled = true;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<BaseEnemy>())
        {
            collision.gameObject.GetComponent<BaseEnemy>().TakeDamage(Damage);
        }
    }
}
