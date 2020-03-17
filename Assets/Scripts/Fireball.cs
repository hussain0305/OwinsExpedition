using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour {

    public float Lifetime;
    public float FireballSpeed;

    private Rigidbody2D Body;
    
    // Use this for initialization
    void Awake () {
        Body = GetComponent<Rigidbody2D>();
        Body.velocity = new Vector2(0.0f, -FireballSpeed);
    }

    public void OnEnable()
    {
        Body.velocity = new Vector2(0.0f, -FireballSpeed);
        Invoke("DestroyThyself", Lifetime);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerAttackProjectile>() && GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().GetProjectilesDestroyEachOther())
        {
            //OPTChange - Destroy(collision.gameObject);
            this.gameObject.SetActive(false);
        }
    }

    public void DestroyThyself()
    {
        this.gameObject.SetActive(false);
    }
}
