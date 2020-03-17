using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPhoenix : BaseEnemy {

    // Use this for initialization
    //public Animator AnimController;
    public GameObject FieryProjection;
    public float CooldownPeriod = 1.0f;
    public AudioClip HoverSound;
    public AudioClip AttackSound;

    private int MarkerCount;
    private bool Idle = true;
    private bool FlapAttack = false;
    private bool ReachedStopPosition;
    private bool AttackedLastTime;
    private bool PhoenixCoroutineRunning = false;
    private float TimeSinceActionStart = 0.0f;
    private Coroutine FlyingCoroutine;
    private GameObject ScreenMarkers;
    private Rigidbody2D Body;



    new void Awake() {
        base.Awake();
        ReachedStopPosition = false;
        AttackedLastTime = false;
        Body = GetComponent<Rigidbody2D>();
        ScreenMarkers = GameObject.FindGameObjectWithTag("ScreenSpaceMarkers");
        MarkerCount = ScreenMarkers.transform.childCount;

        PlayIdleSFX(HoverSound);
    }
	
    new void OnEnable()
    {
        base.OnEnable();
        ReachedStopPosition = false;
        AttackedLastTime = false;
        Idle = true;
        FlapAttack = false;
        PhoenixCoroutineRunning = false;

        PlayIdleSFX(HoverSound);
    }

    IEnumerator UpdateSubstitute()
    {
        while (this.gameObject.activeSelf)
        {
            if (Idle)
            {
                DecideWhatToDo();
            }
            yield return new WaitForSeconds(CooldownPeriod);
        }
    }

    void RangedAttack()
    {
        FlapAttack = true;
        AnimController.SetBool("FlapAttack", FlapAttack);
        Idle = false;
        AttackedLastTime = true;

    }

    public void PlayFieryAttackSound()
    {
        PlayAttackSFX(AttackSound);
    }

    void FlyAway()
    {
        int RandomSpot = Random.Range(0, MarkerCount);
        Vector2 SpotLocation = ScreenMarkers.transform.GetChild(RandomSpot).transform.position;
        Idle = false;
        AttackedLastTime = false;
        FlyingCoroutine = StartCoroutine(MotionRoutine(SpotLocation, 3));
    }

    public void StopRangedAttack()
    {
        FlapAttack = false;
        Idle = true;
        AnimController.SetBool("FlapAttack", FlapAttack);
        TimeSinceActionStart = 0.0f;
    }

    public void SpawnProjectile()
    {
        //OPTChange - Instantiate(FieryProjection, (transform.position + new Vector3(0.0f, -1.0f)), Quaternion.identity);
        ObjectPooler.CentralObjectPool.SpawnFromPool(FieryProjection.name, (transform.position + new Vector3(0.0f, -1.0f)), Quaternion.identity);
    }
    void DecideWhatToDo()
    {
        if (!AttackedLastTime && Random.Range(0, 10) < 8)
        {
            RangedAttack();
        }
        else
        {
            FlyAway();
        }
    }

    void PositionCorrection(Vector2 CorrectPosition)
    {
        transform.position = CorrectPosition;
        Idle = true;
        if(PhoenixCoroutineRunning)
            StopCoroutine(FlyingCoroutine);
    }

    public void SetReachedStopPosition()
    {
        StartCoroutine(UpdateSubstitute());
    }

    IEnumerator MotionRoutine(Vector2 Destination, float DashSpeed)
    {
        PhoenixCoroutineRunning = true;
        while (Vector2.Distance(transform.position, Destination) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, DashSpeed * Time.deltaTime);
            yield return null;
        }
        PhoenixCoroutineRunning = false;
        PositionCorrection(Destination);
        
        yield return new WaitForSeconds(2);
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
