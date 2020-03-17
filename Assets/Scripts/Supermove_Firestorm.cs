using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supermove_Firestorm : BaseSupermove
{
    public AudioClip SupermoveSound;

    private AudioSource SupermoveSoundSource;

    // Use this for initialization
    new void Start () {
        base.Start();
    }

    public override void PerformSuperMove()
    {
        InitializeSounds();
        SupermoveSoundSource.Play();
        GameObject[] CurrentEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach(GameObject CurrentEnemy in CurrentEnemies)
        {
            Instantiate(this.gameObject, CurrentEnemy.transform.position, CurrentEnemy.transform.rotation);
            //ObjectPooler.CentralObjectPool.SpawnFromPool(this.gameObject.name, CurrentEnemy.transform.position, CurrentEnemy.transform.rotation);
        }
    }

    public void InitializeSounds()
    {
        if (!SupermoveSoundSource)
        {
            SupermoveSoundSource = gameObject.AddComponent<AudioSource>();
            SupermoveSoundSource.clip = SupermoveSound;
        }
    }

    public void DestroyEffectRemnant()
    {
        Destroy(this.gameObject);
    }

}
