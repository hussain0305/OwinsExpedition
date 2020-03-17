using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyHazeShape { Square, Circle };

public class EnemyProjectileHazer : MonoBehaviour
{
    public int NumberOfParticles = 10;
    public float SpawnsPerSecond = 10;
    public float SpawnOffsetRangeX = 0.2f;
    public float SpawnOffsetRangeY = 0.2f;
    public Color PrimaryHazeColor;
    public Color SecondaryHazeColor;
    public EnemyHazeShape EffectShape = EnemyHazeShape.Circle;
    public GameObject HazeParticle;
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
            case EnemyHazeShape.Circle:
                bool Redo = true;
                while (Redo)
                {
                    X = transform.position.x + Random.Range(-SpawnOffsetRangeX, SpawnOffsetRangeX);
                    Y = transform.position.y + Random.Range(-SpawnOffsetRangeY, SpawnOffsetRangeY);
                    if (Vector2.Distance(new Vector2(X, Y), transform.position) <= SpawnOffsetRangeX)
                    {
                        Redo = false;
                    }
                }
                break;

            case EnemyHazeShape.Square:
                X = transform.position.x + Random.Range(-SpawnOffsetRangeX, SpawnOffsetRangeX);
                Y = transform.position.y + Random.Range(-SpawnOffsetRangeY, SpawnOffsetRangeY);
                break;
        }

        return new Vector2(X, Y);
    }

    public void SpawnParticles()
    {
        for(int loop = 0; loop < NumberOfParticles; loop++)
        {
            HP = ObjectPooler.CentralObjectPool.SpawnFromPool(HazeParticle.name, GetSpawnLocation(), transform.rotation);
            HP.GetComponent<FireParticle_Enemy>().SetParticleColor(Random.Range(1, 10) < 5 ? PrimaryHazeColor : SecondaryHazeColor);
        }
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
