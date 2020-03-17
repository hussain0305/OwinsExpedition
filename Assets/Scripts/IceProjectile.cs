using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour {

    public float IceSpeed;
    public GameObject Icicle;

    private Rigidbody2D Body;
    private int Multiplier = 0;
    
    // Use this for initialization
    void Awake () {
        Body = GetComponent<Rigidbody2D>();
        if(transform.position.x < -1)
        {
            Body.velocity = new Vector2(IceSpeed / 2, -IceSpeed);
            Multiplier = -1;
        }

        else
        {
            Body.velocity = new Vector2(-IceSpeed / 2, -IceSpeed);
            Multiplier = 1;
        }
    }

    private void OnEnable()
    {
        Multiplier = 0;
        if (transform.position.x < -1)
        {
            Body.velocity = new Vector2(IceSpeed / 2, -IceSpeed);
            Multiplier = -1;
        }

        else
        {
            Body.velocity = new Vector2(-IceSpeed / 2, -IceSpeed);
            Multiplier = 1;
        }
    }

    // Update is called once per frame
    void Update() {
        if ((Multiplier * transform.position.x) <= 0)
            SpawnIcicle();
    }

    private void SpawnIcicle()
    {
        //OPTChange - Instantiate(Icicle, new Vector2(0, transform.position.y), Quaternion.identity);
        //OPTChange - Destroy(this.gameObject);

        ObjectPooler.CentralObjectPool.SpawnFromPool(Icicle.name, new Vector2(0, transform.position.y), Quaternion.identity);
        this.gameObject.SetActive(false);
    }
}
