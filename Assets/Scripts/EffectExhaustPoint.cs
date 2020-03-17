using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectExhaustPoint : MonoBehaviour
{
    public int NumberOfParticles = 1;
    public float SpawnsPerSecond = 50;
    public float SpawnOffsetRangeX = 0.2f;
    public float SpawnOffsetRangeY = 0.2f;
    public GameObject HazeParticleFire;
    public GameObject HazeParticleIce;
    public GameObject HazeParticleMagic;

    private int EffectIndex = 0;
    private float TimePassed = 0;
    private GameObject ParticleToSpawn;
    private GameObject SpawnedParticle;

    // Update is called once per frame
    void Update()
    {
        if (TimePassed > 1 / SpawnsPerSecond)
        {
            SpawnParticles();
        }
        else
        {
            TimePassed += Time.deltaTime;
        }
    }

    Vector2 GetSpawnLocation()
    {
        float X, Y;
        X = transform.position.x + Random.Range(-SpawnOffsetRangeX, SpawnOffsetRangeX);
        Y = transform.position.y + Random.Range(-SpawnOffsetRangeY, SpawnOffsetRangeY);

        return new Vector2(X, Y);
    }

    public void SpawnParticles()
    {
        switch (EffectIndex)
        {
            case 0:
                break;
            case 1:
                for (int x = 0; x < NumberOfParticles; x++)
                {
                    //SpawnedParticle = Instantiate(HazeParticleIce, GetSpawnLocation(), transform.rotation);
                    SpawnedParticle = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticleIce.name, GetSpawnLocation(), transform.rotation);
                    //SpawnedParticle.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(1, 10) < 5 ? 1 : 3;
                }
                break;
            case 2:
                for (int x = 0; x < NumberOfParticles; x++)
                {
                    //SpawnedParticle = Instantiate(HazeParticleFire, GetSpawnLocation(), transform.rotation);
                    SpawnedParticle = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticleFire.name, GetSpawnLocation(), transform.rotation);
                    //SpawnedParticle.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(1, 10) < 5 ? 1 : 3;
                }
                break;
            case 3:
                for (int x = 0; x < NumberOfParticles; x++)
                {
                    //SpawnedParticle = Instantiate(HazeParticleMagic, GetSpawnLocation(), transform.rotation);
                    SpawnedParticle = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticleMagic.name, GetSpawnLocation(), transform.rotation);
                    //SpawnedParticle.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(1, 10) < 5 ? 1 : 3;
                }
                break;
        }
    }

    public void SetCurrentElement(int Element)
    {
        EffectIndex = Element;
    }
}
