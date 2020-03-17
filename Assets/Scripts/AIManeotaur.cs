using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManeotaur : BaseEnemy {

    //public Animator AnimController;
    public AudioClip IdleSound;
    public AudioClip SlamSound;

    private GameObject Manager;
    private Rigidbody2D Body;
    private bool Idle = true;
    private float CooldownPeriod = 2.0f;

    // Use this for initialization
    new void Awake()
    {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();
        //PlayIdleSFX(IdleSound);
        Manager = GameObject.FindGameObjectWithTag("WorldManager");
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
                SlamAttack();
            }
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }

    void SlamAttack()
    {
        Idle = false;
        AnimController.SetTrigger("SlamAttack");
    }

    public void PlaySlamSound()
    {
        PlayAttackSFX(SlamSound);
    }

    public void FinishSlamAttack()
    {
        Idle = true;
        SpeedDown();
    }

    public void SpeedUp()
    {
        Body.velocity = new Vector2(0.0f, -8);
    }

    void SpeedDown()
    {
        Body.velocity = new Vector2(0.0f, -2.25f);
    }

    public void SlamShake()
    {
        Manager.GetComponent<CameraControl>().ShakeDefault();
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
