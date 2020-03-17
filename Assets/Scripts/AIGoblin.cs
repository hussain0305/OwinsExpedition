using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGoblin : BaseEnemy {

    //public Animator AnimController;
    public GameObject MinionSpawner;
    public AudioClip GoblinAttack;

    private bool Idle = true;
    private float CooldownPeriod = 1;
    private Rigidbody2D Body;
    private GameObject Spawner;

    // Use this for initialization
    new void Awake()
    {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();
    }

    new void OnEnable()
    {
        base.OnEnable();
        Idle = true;
        CooldownPeriod = 1.25f;
        if (this.gameObject.activeSelf)
            StartCoroutine(UpdateSubstitute());
    }

    IEnumerator UpdateSubstitute()
    {
        yield return new WaitForSeconds(CooldownPeriod);
        CooldownPeriod = 2.25f;
        while (this.gameObject.activeSelf)
        {
            if (Idle)
            {
                SpawnMinionSpawner();
            }
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }
    
    void SpawnMinionSpawner()
    {
        Idle = false;
        AnimController.SetTrigger("SpawnMinions");
        PlayAttackSFX(GoblinAttack);
        Spawner = ObjectPooler.CentralObjectPool.SpawnFromPool(MinionSpawner.name, transform.position, Quaternion.identity);
        Spawner.transform.SetParent(gameObject.transform);
    }

    public void TriggerMinionRise()
    {
        Spawner.GetComponent<MinionSpawner>().SpawnMinions();
        Idle = true;
        //CooldownPeriod+=0.75f;
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
