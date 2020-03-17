using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBolt : MonoBehaviour {

    public float Lifetime;
    public float BoltSpeed;

    private Rigidbody2D Body;
    // Use this for initialization
    void Awake () {
        Body = GetComponent<Rigidbody2D>();
        Body.velocity = new Vector2(0.0f, -BoltSpeed);
    }

    public void OnEnable()
    {
        Invoke("DestroyBolt", Lifetime);
        if(Body)
            Body.velocity = new Vector2(0.0f, -BoltSpeed);
    }

    public void DestroyBolt()
    {
        this.gameObject.SetActive(false);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerAttackProjectile>() && GameObject.FindGameObjectWithTag("WorldManager").GetComponent<WorldManager>().GetProjectilesDestroyEachOther())
        {
            //OPTChange - Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
