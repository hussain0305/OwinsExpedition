using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour {

    public GameObject Minions;

    private Coroutine SpawningMinnions;

    public void SpawnMinions()
    {
        SpawningMinnions = StartCoroutine(SpawnMinionsWithOffset());
    }

    void SpawnerDie()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }

    IEnumerator SpawnMinionsWithOffset()
    {
        foreach (Transform MinionPosition in transform)
        {
            if (Random.Range(0, 10) > 5)
            {
                //GameObject SpawnedMinion = Instantiate(Minions, MinionPosition.transform.position, MinionPosition.transform.rotation);
                GameObject SpawnedMinion = ObjectPooler.CentralObjectPool.SpawnFromPool(Minions.name, MinionPosition.transform.position, MinionPosition.transform.rotation);
                SpawnedMinion.GetComponent<Rigidbody2D>().velocity = SpawnedMinion.transform.up * -4.0f;
                yield return new WaitForSeconds(0.1f);
            }
            
        }
        SpawnerDie();
        yield return new WaitForSeconds(2);
    }


}
