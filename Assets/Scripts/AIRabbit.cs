using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRabbit : BaseEnemy {

    public float LungeSpeed;
    public float WalkSpeed;
    public float MaxLife = 4;
    public AudioClip WalkSound;
    public AudioClip HopSound;
    public AudioClip AttackSound;

    private int NumberOfHops;
    private bool RabbitCoroutineRunning;
    private GameObject VFX;
    private Rigidbody2D Body;

    private Coroutine RabbitCoroutine;

    private RabbitObstacleSensor ROS;
    private RabbitPlayerSensor RPS;

    int LaneNumber;

    // Use this for initialization
    new void Awake() {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();

        AssignLane();

        NumberOfHops = 0;

        PlayIdleSFX(WalkSound);
        SetIdleVolume(0.4f);

        Body.velocity = new Vector2(0.0f, -WalkSpeed);

        ROS = GetComponentInChildren<RabbitObstacleSensor>();
        RPS = GetComponentInChildren<RabbitPlayerSensor>();
    }

    new void OnEnable()
    {
        base.OnEnable();
        AssignLane();
        NumberOfHops = 0;

        Body.velocity = new Vector2(0.0f, -WalkSpeed);

        StopAllCoroutines();

        StartCoroutine(CountdownToDeath());
        StartCoroutine(BugsOOSDeath());

        StartRegisteringNewCollisions();
    }
	
    public void RefreshForwardSpeed()
    {
        Body.velocity = new Vector2(0.0f, -WalkSpeed);
    }

    void AssignLane()
    {
        if (transform.position.x < -1)
        {
            LaneNumber = -1;
        }
        else if (transform.position.x > 1)
        {
            LaneNumber = 1;
        }
        else
        {
            LaneNumber = 0;
        }
    }

    void RabbitHopLeft()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        NumberOfHops++;
        CheckIfHopsExceeded();

        PlayAttackSFX(HopSound);
        AnimController.SetTrigger("HopLeft");
        Vector3 Destination = transform.position + new Vector3(-1.5f, 0, 0.0f);
        
        if (RabbitCoroutineRunning)
        {
            StopCoroutine(RabbitCoroutine);
        }
        
        //StopAllCoroutines();
        Body.velocity = new Vector2(0.0f, 0.0f);
        RabbitCoroutine = StartCoroutine(MotionRoutine(Destination, 10.0f));
    }

    void RabbitHopRight()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        NumberOfHops++;
        CheckIfHopsExceeded();

        PlayAttackSFX(HopSound);
        AnimController.SetTrigger("HopRight");
        Vector3 Destination = transform.position + new Vector3(1.5f, 0, 0.0f);
        
        if (RabbitCoroutineRunning)
        {
            StopCoroutine(RabbitCoroutine);
        }
            
        Body.velocity = new Vector2(0.0f, 0.0f);
        RabbitCoroutine = StartCoroutine(MotionRoutine(Destination, 10.0f));
    }

    void CheckIfHopsExceeded()
    {
        if (NumberOfHops > 2)
        {
            StopAllCoroutines();
            DestroyEnemy();
        }
    }

    void PositionCorrection(Vector2 Desti)
    {
        transform.position = Desti;
    }

    public void ObstacleAhead()
    {
        AssignLane();
        StopRegisteringNewCollisions();

        if (LaneNumber == -1)
        {
            RabbitHopRight();
        }
        else if (LaneNumber == 1)
        {
            RabbitHopLeft();
        }
        else
        {
            if (Random.Range(0, 11) < 6)
                RabbitHopRight();
            else
                RabbitHopLeft();
        }
    }

    public void PlayerAhead()
    {
        StopRegisteringNewCollisions();

        AnimController.SetTrigger("Attack");
    }

    public void Lunge()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        PlayAttackSFX(AttackSound);
        Body.velocity = new Vector2(0.0f, -LungeSpeed);
    }

    public void StopRegisteringNewCollisions()
    {
        ROS.SetAlreadyRegistered(true);
        RPS.SetAlreadyRegistered(true);
        Invoke("StartRegisteringNewCollisions", 0.5f);
    }

    public void StartRegisteringNewCollisions()
    {
        ROS.SetAlreadyRegistered(false);
        RPS.SetAlreadyRegistered(false);
    }

    IEnumerator MotionRoutine(Vector2 Destination, float DashSpeed)
    {
        yield return new WaitForSeconds(0.1f);
        RabbitCoroutineRunning = true;
        while (Vector2.Distance(transform.position, Destination) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, DashSpeed * Time.deltaTime);
            yield return null;
        }
        PositionCorrection(Destination);
        yield return new WaitForSeconds(0.1f);
        RefreshForwardSpeed();
        AssignLane();
        RabbitCoroutineRunning = false;
    }

    IEnumerator CountdownToDeath()
    {
        yield return new WaitForSeconds(MaxLife);
        DestroyEnemy();
    }

    IEnumerator BugsOOSDeath()
    {
        while (this.gameObject.activeSelf)
        {
            if (this.gameObject.transform.position.y < -5.5f)
            {
                DestroyEnemy();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public override void DestroyEnemy()
    {
        if (this.gameObject.activeSelf)
        {
            VFX = ObjectPooler.CentralObjectPool.SpawnFromPool(DeathVFX.name, (transform.position - new Vector3(0, 0.6f)), transform.rotation);
            VFX.GetComponent<SpriteRenderer>().color = SkinTone;

            StopAllCoroutines();
            this.gameObject.SetActive(false);
        }
    }
}
