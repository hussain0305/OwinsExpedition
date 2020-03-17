using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPikeman : BaseEnemy{

    //public Animator AnimController;
    public GameObject Pike;
    public AudioClip IdleSound;
    public AudioClip AttackSound;

    private Rigidbody2D Body;
    private float CooldownPeriod = 1;
    private float TimeSinceLastPikeThrow = 0.0f;

    // Use this for initialization
    new void Awake()
    {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();
        //PlayIdleSFX(IdleSound);
    }

    new void OnEnable()
    {
        base.OnEnable();
        PlayIdleSFX(IdleSound);
        if (this.gameObject.activeSelf)
            StartCoroutine(UpdateSubstitute());
    }

    IEnumerator UpdateSubstitute()
    {
        yield return new WaitForSeconds(CooldownPeriod);
        while (this.gameObject.activeSelf)
        {
            DoPikeAttack();
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }

    void DoPikeAttack()
    {
        AnimController.SetTrigger("ThrowPike");
    }

    public void PikeSound()
    {
        PlayAttackSFX(AttackSound);
    }
    
    public void SpawnProjectile()
    {
        //OPTChange - GameObject SpawnedPike = Instantiate(Pike, (transform.position + new Vector3(0.0f, -1.0f)), transform.rotation * Quaternion.Euler(0f, 0f, 180f));
        GameObject SpawnedPike = ObjectPooler.CentralObjectPool.SpawnFromPool(Pike.name, (transform.position + new Vector3(0.0f, -1.0f)), transform.rotation * Quaternion.Euler(0f, 0f, 180f));
        SpawnedPike.GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, -8);
        AnimController.SetTrigger("Idle");
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
