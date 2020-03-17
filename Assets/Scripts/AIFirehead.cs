using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFirehead : BaseEnemy
{
    public GameObject Fireball;
    public AudioClip IdleSound;
    public AudioClip AttackSound;

    private Rigidbody2D Body;
    private bool Idle = true;
    private bool FireballAttack = false;
    private float CooldownPeriod = 0.75f;

    // Use this for initialization
    new void Awake() {
        base.Awake();

        Body = GetComponent<Rigidbody2D>();

        if (this.gameObject.activeSelf)
            StartCoroutine(UpdateSubstitute());
    }

    new void OnEnable()
    {
        base.OnEnable();
        PlayIdleSFX(IdleSound);
        Idle = true;
        if (this.gameObject.activeSelf)
            StartCoroutine(UpdateSubstitute());
    }

    IEnumerator UpdateSubstitute()
    {
        yield return new WaitForSeconds(CooldownPeriod);
        while (this.gameObject.activeSelf)
        {
            if (Idle)
            {
                FireballRangedAttack();
            }
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }

    void FireballRangedAttack()
    {
        FireballAttack = true;
        AnimController.SetBool("ThrewFireball", FireballAttack);
        Idle = false;
    }

    public void PlayFireguyAttackSound()
    {
        PlayAttackSFX(AttackSound);
    }

    public void StopRangedAttack()
    {
        FireballAttack = false;
        Idle = true;
        AnimController.SetBool("ThrewFireball", FireballAttack);
    }
    public void SpawnProjectile()
    {
        ObjectPooler.CentralObjectPool.SpawnFromPool(Fireball.name, (transform.position + new Vector3(0, -1.0f)), Quaternion.identity);
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
