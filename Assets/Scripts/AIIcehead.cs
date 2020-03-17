using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIIcehead : BaseEnemy
{

    //public Animator AnimController;
    public GameObject Iceball;
    public AudioClip ThrowSound;

    private Rigidbody2D Body;
    private bool Idle;
    private float CooldownPeriod = 0.75f;

    new void Awake() {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();

        Idle = true;

        if (transform.position.x < 1 && transform.position.x > -1)
        {
            //OPTChange - Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        else if(transform.position.x > 1)
        {
        }
        else
        {
            GetSpriteRenderer().flipX = true;
        }
        if(this.gameObject.activeSelf)
            StartCoroutine(UpdateSubstitute());
    }
	
    new void OnEnable()
    {
        base.OnEnable();
        GetSpriteRenderer().flipX = false;
        Idle = true;

        if (transform.position.x < 1 && transform.position.x > -1)
        {
            //OPTChange - Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
        else if (transform.position.x > 1)
        {
        }
        else
        {
            GetSpriteRenderer().flipX = true;
        }
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
                IceballRangedAttack();
            } 
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }

    void IceballRangedAttack()
    {
        Idle = false;
        AnimController.SetTrigger("ThrewProjectile");
        Invoke("DelayedSoundTesting", 0.4f);
    }

    void DelayedSoundTesting()
    {
        PlayAttackSFX(ThrowSound);
    }

    public void SpawnProjectile()
    {
        ObjectPooler.CentralObjectPool.SpawnFromPool(Iceball.name, (transform.position + new Vector3(0.0f, -1.0f)), Quaternion.identity);
    }

    public void ResetBools()
    {
        Idle = true;
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
