using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSpit : MonoBehaviour {

    public float Lifetime;
    public float SpitSpeed;
    public GameObject Sludge;

    private float Age = 0;
    private Animator AnimController;
    private Rigidbody2D Body;

    // Use this for initialization

    void Start()
    {
        Body = GetComponent<Rigidbody2D>();
        Body.velocity = new Vector2(0.0f, -SpitSpeed);
        AnimController = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        Age = 0;
        if(AnimController)
            AnimController.SetTrigger("SpitRestart");
        else
            AnimController = GetComponent<Animator>();

        //Body.velocity = new Vector2(0.0f, -SpitSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Age += Time.deltaTime;
        if (Age > Lifetime)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void SpawnSludge()
    {
        //OPTChange - Instantiate(Sludge, transform.position, Quaternion.identity);
        ObjectPooler.CentralObjectPool.SpawnFromPool(Sludge.name, transform.position, Quaternion.identity);
    }
}
