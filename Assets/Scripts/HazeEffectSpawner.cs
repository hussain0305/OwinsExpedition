using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HazeShape { Square, Circle };

public class HazeEffectSpawner : MonoBehaviour
{
    public int NumberOfParticles = 1;
    public float SpawnsPerSecond = 50;
    public float SpawnOffsetRangeX = 0.2f;
    public float SpawnOffsetRangeY = 0.2f;
    public Color HazeColorIce;
    public Color HazeColorFire;
    public Color HazeColorMagic;
    public HazeShape EffectShape = HazeShape.Circle;
    public GameObject HazeParticle;
    private int EffectIndex = 0;
    private GameObject ParticleToSpawn;
    private GameObject SpawnedParticle;
    private GameObject HP;

    private void OnEnable()
    {
        StopAllCoroutines();
        StartCoroutine(ParticleSpawning());
    }

    public void OnDisable()
    {
        StopAllCoroutines();
    }

    Vector2 GetSpawnLocation()
    {
        float X, Y;
        X = transform.position.x + Random.Range(-SpawnOffsetRangeX, SpawnOffsetRangeX);
        Y = transform.position.y + Random.Range(-SpawnOffsetRangeY, SpawnOffsetRangeY);
        switch (EffectShape)
        {
            case HazeShape.Circle:
                bool Redo = true;
                while (Redo)
                {
                    X = transform.position.x + Random.Range(-SpawnOffsetRangeX, SpawnOffsetRangeX);
                    Y = transform.position.y + Random.Range(-SpawnOffsetRangeY, SpawnOffsetRangeY);
                    if(Vector2.Distance(new Vector2(X, Y), transform.position) <= SpawnOffsetRangeX)
                    {
                        Redo = false;
                    }
                }
                break;

            case HazeShape.Square:
                X = transform.position.x + Random.Range(-SpawnOffsetRangeX, SpawnOffsetRangeX);
                Y = transform.position.y + Random.Range(-SpawnOffsetRangeY, SpawnOffsetRangeY);
                break;
        }

        return new Vector2(X, Y);
    }

    public void SpawnParticles()
    {
        switch (EffectIndex)
        {
            case 0:
                break;

            case 1:
                for(int x = 0; x < NumberOfParticles; x++)
                {
                    HP = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticle.name, GetSpawnLocation(), transform.rotation);
                    HP.GetComponent<FireParticle>().SetParticleColor(HazeColorIce);
                    //if (GetComponentInParent<Rigidbody2D>())
                    //{
                        //HP.GetComponent<FireParticle>().SetParticleVelocity(GetComponentInParent<Rigidbody2D>().velocity / 25);
                    //}
                }
                break;

            case 2:
                for (int x = 0; x < NumberOfParticles; x++)
                {
                    HP = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticle.name, GetSpawnLocation(), transform.rotation);
                    HP.GetComponent<FireParticle>().SetParticleColor(HazeColorFire);
                    //if (GetComponentInParent<Rigidbody2D>())
                    //{
                        //HP.GetComponent<FireParticle>().SetParticleVelocity(GetComponentInParent<Rigidbody2D>().velocity / 25);
                    //}
                }
                break;

            case 3:
                for (int x = 0; x < NumberOfParticles; x++)
                {
                    HP = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticle.name, GetSpawnLocation(), transform.rotation);
                    HP.GetComponent<FireParticle>().SetParticleColor(HazeColorMagic);
                    //if (GetComponentInParent<Rigidbody2D>())
                    //{
                        //HP.GetComponent<FireParticle>().SetParticleVelocity(GetComponentInParent<Rigidbody2D>().velocity / 25);
                    //}
                }
                break;
        }      
    }

    public void SetCurrentElement(int Element)
    {
        EffectIndex = Element;
    }

    IEnumerator ParticleSpawning()
    {
        yield return new WaitForSeconds(1 / SpawnsPerSecond);
        while (true)
        {
            SpawnParticles();
            yield return new WaitForSeconds(1 / SpawnsPerSecond);
        }
    }
}
