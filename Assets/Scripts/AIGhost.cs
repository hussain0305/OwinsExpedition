using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIGhost : BaseEnemy
{

    // Use this for initialization
    //public Animator AnimController;
    public GameObject Bolt;
    public AudioClip IdleSound;
    public AudioClip MeleeSound;
    public AudioClip RangedSound;

    private bool Idle = true;
    private bool Melee = false;
    private bool Ranged = false;
    private bool GoingTowardsPlayer = false;
    private bool GhostRoutineRunning = false;
    private float CooldownPeriod = 2.0f;
    //private float TimeSinceActionStart = 0.0f;
    private Rigidbody2D Body;
    private Coroutine GhostRoutine;
    private Vector3 StartingPositionRanged;
    private Vector3 StartingPositionMelee;

    new void Awake()
    {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();
        Idle = true;
        if (this.gameObject.activeSelf)
            StartCoroutine(UpdateSubstitute());
        //PlayIdleSFX(IdleSound);
    }

    new void OnEnable() {
        base.OnEnable();
        Idle = true;
        Melee = false;
        Ranged = false;
        GoingTowardsPlayer = false;
        GhostRoutineRunning = false;
        PlayIdleSFX(IdleSound);
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
                Attack();
            }
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }

    void SetCooldownPeriod(float Cooldown)
    {
        CooldownPeriod = Cooldown;
    }

    void MeleeAttack()
    {
        Melee = true;
        Idle = false;
        AnimController.SetBool("Melee", Melee);
        PlayAttackSFX(MeleeSound);
    }
    void RangedAttack()
    {
        Ranged = true;
        Idle = false;
        AnimController.SetBool("Ranged", Ranged);
        PlayAttackSFX(RangedSound);
    }
    void Attack()
    {
        if(Random.Range(0, 10) < 5)
        {
            MeleeAttack();
        }
        else
        {
            RangedAttack();
        }
    }

    public void SpawnProjectile()
    {
        //OPTChange - Instantiate(Bolt, (transform.position + new Vector3(0.25f, -1.0f)), Quaternion.identity);
        ObjectPooler.CentralObjectPool.SpawnFromPool(Bolt.name, (transform.position + new Vector3(0.25f, -1.0f)), Quaternion.identity);
    }

    void MoveRight()
    {
        Vector3 Destination = transform.position + new Vector3(1.5f, 0, 0.0f);
        if (GhostRoutineRunning)
        {
            StopCoroutine(GhostRoutine);
        }
        GhostRoutine = StartCoroutine(MotionRoutine(Destination, 10.0f));
    }
    void MoveLeft()
    {
        Vector3 Destination = transform.position + new Vector3(-1.5f, 0, 0.0f);
        if (GhostRoutineRunning)
        {
            StopCoroutine(GhostRoutine);
        }
        GhostRoutine = StartCoroutine(MotionRoutine(Destination, 10.0f));
    }
    void Halt()
    {
        Body.velocity = new Vector2(0.0f, 0.0f);
    }

    public void FinishAttack()
    {
        Melee = false;
        Ranged = false;
        Idle = true;
        AnimController.SetBool("Melee", Melee);
        AnimController.SetBool("Ranged", Ranged);
        //TimeSinceActionStart = 0.0f;
    }
    public void StartDash()
    {
        StartingPositionRanged = transform.position;
        if(StartingPositionRanged.x < -1)
        {
            MoveRight();
        }
        else if(StartingPositionRanged.x > 1)
        {
            MoveLeft();
        }
        else
        {
            if (Random.Range(0, 10) < 6)
                MoveRight();
            else
                MoveLeft();
        }
    }

    public void MeleeDash()
    {
        if (!GoingTowardsPlayer)
        {
            GoingTowardsPlayer = true;
            //Go to Player
            StartingPositionMelee = transform.position;
            Vector3 Destination = GameObject.FindGameObjectWithTag("Player").transform.position;
            Destination = Destination - new Vector3(0.0f, -0.5f, 0.0f);
            if (GhostRoutineRunning)
            {
                StopCoroutine(GhostRoutine);
            }
            GhostRoutine = StartCoroutine(MotionRoutine(Destination, 7.5f));
        }
        else
        {
            GoingTowardsPlayer = false;
            //Go back to original position
            if (GhostRoutineRunning)
            {
                StopCoroutine(GhostRoutine);
            }
            GhostRoutine = StartCoroutine(MotionRoutine(StartingPositionMelee, 7.5f));
        }
    }

    void AdjustPosition(Vector2 Destination)
    {
        transform.position = Destination;
        if(GhostRoutineRunning)
            StopCoroutine(GhostRoutine);
    }

    IEnumerator MotionRoutine(Vector2 Destination, float DashSpeed)
    {
        GhostRoutineRunning = true;
        while(Vector2.Distance(transform.position, Destination) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, DashSpeed * Time.deltaTime);
            yield return null;
        }
        GhostRoutineRunning = false;
        AdjustPosition(Destination);
        yield return new WaitForSeconds(2);
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
