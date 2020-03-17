using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBat : BaseEnemy {

    // Use this for initialization
    //public Animator AnimController;
    public GameObject BatSpit;
    public float CooldownPeriod = 1.0f;
    public AudioClip FlyingBuzz;
    public AudioClip SpitSound;

    private int MarkerCount;
    private bool Idle = true;
    //private bool ReachedStopPosition;
    private bool AttackedLastTime;
    private bool BatCoroutineRunning = false;
    private Coroutine FlyingCoroutine;
    private GameObject ScreenMarkers;
    private Rigidbody2D Body;



    new void Awake()
    {
        base.Awake();
        AttackedLastTime = false;
        Body = GetComponent<Rigidbody2D>();
        ScreenMarkers = GameObject.FindGameObjectWithTag("ScreenSpaceMarkers");
        MarkerCount = ScreenMarkers.transform.childCount;
    }

    new void OnEnable()
    {
        base.OnEnable();
        AttackedLastTime = false;
        PlayIdleSFX(FlyingBuzz);
        Idle = true;
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

    void SpitAttack()
    {
        AnimController.SetTrigger("ThrewSpit");
        Idle = false;
        AttackedLastTime = true;

        PlayAttackSFX(SpitSound);
    }

    void FlyAway()
    {
        int RandomSpot = Random.Range(0, MarkerCount);
        Vector2 SpotLocation = ScreenMarkers.transform.GetChild(RandomSpot).transform.position;
        Idle = false;
        AttackedLastTime = false;
        FlyingCoroutine = StartCoroutine(MotionRoutine(SpotLocation, 3));
    }

    public void StopSpitAttack()
    {
        Idle = true;
    }
    public void SpawnProjectile()
    {
        ObjectPooler.CentralObjectPool.SpawnFromPool(BatSpit.name, (transform.position + new Vector3(0.25f, -1.0f)), Quaternion.identity);
    }
    void DecideWhatToDo()
    {
        if (!AttackedLastTime && Random.Range(0, 10) < 8)
        {
            SpitAttack();
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
        if(BatCoroutineRunning)
            StopCoroutine(FlyingCoroutine);
    }
    
    public void SetReachedStopPosition()
    {
        StartCoroutine(UpdateSubstitute());
    }

    IEnumerator MotionRoutine(Vector2 Destination, float DashSpeed)
    {
        BatCoroutineRunning = true;
        while (Vector2.Distance(transform.position, Destination) > 0.05f)
        {
            transform.position = Vector2.Lerp(transform.position, Destination, DashSpeed * Time.deltaTime);
            yield return null;
        }
        BatCoroutineRunning = false;
        PositionCorrection(Destination);
        yield return new WaitForSeconds(2);
    }

    public override void DestroyEnemy()
    {
        StopAllCoroutines();
        this.gameObject.SetActive(false);
    }
}
