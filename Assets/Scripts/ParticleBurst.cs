using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    public GameObject Particle_1;
    public GameObject Particle_2;
    public GameObject Particle_3;

    private float Life = 0;
    private float Age = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartBurst());
    }

    IEnumerator StartBurst()
    {
        while (Life < Age)
        {
            Life += Time.deltaTime;
            //Instantiate(Particle_1, transform.position, transform.rotation);
            //Instantiate(Particle_2, transform.position, transform.rotation);
            //Instantiate(Particle_3, transform.position, transform.rotation);
            ObjectPooler.CentralObjectPool.SpawnFromPool(Particle_1.name, transform.position, transform.rotation);
            ObjectPooler.CentralObjectPool.SpawnFromPool(Particle_2.name, transform.position, transform.rotation);
            ObjectPooler.CentralObjectPool.SpawnFromPool(Particle_3.name, transform.position, transform.rotation);

            yield return new WaitForEndOfFrame();
        }

        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
