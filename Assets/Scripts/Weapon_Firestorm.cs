using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Firestorm : BaseWeapon {

    // Use this for initialization
    public GameObject Fireballs;
    public GameObject Crater;
    public float CraterOffsetY = 0.2f;

    private int TimesSpawned;
    private Vector2[] Locations;
    private Vector3 CraterLocation;
    private Vector3 OriginalScale;

    new void Awake()
    {
        base.Awake();
        ResetOrientation();
        TimesSpawned = 0;
        Locations = new Vector2[6];

        Locations[0] = new Vector2(-0.35f, 0.2f);
        Locations[1] = new Vector2(0.35f, 0.2f);
        Locations[2] = new Vector2(-0.35f, 0);
        Locations[3] = new Vector2(0.35f, 0);
        Locations[4] = new Vector2(-0.35f, 0);
        Locations[5] = new Vector2(0.35f, 0);

        OriginalScale = transform.localScale;
    }

    new void OnEnable()
    {
        base.OnEnable();
        ResetOrientation();
        TimesSpawned = 0;
    }

    // Update is called once per frame
    new void Update()
    {
        if (!this.gameObject.activeSelf)
        {
            return;
        }

        TimePassed += Time.deltaTime;
        if (ActionAllowed)
        {
            CurrentFingerPosition = Input.GetTouch(0).position;
            Vector3 diff = Camera.main.ScreenToWorldPoint(new Vector3(CurrentFingerPosition.x, CurrentFingerPosition.y, Camera.main.nearClipPlane));
            transform.position = diff;
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.05f);
    }

    public override void SwipeInitiated(Vector2 SwipeStart)
    {
        SwipeStartAt = SwipeStart;
        transform.localScale = OriginalScale;
    }

    public override void SwipeEnded(Vector2 SwipeEnd)
    {
        SwipeEndAt = SwipeEnd;
        CraterLocation = Camera.main.ScreenToWorldPoint(new Vector3(CurrentFingerPosition.x, CurrentFingerPosition.y, Camera.main.nearClipPlane));
        CraterLocation = new Vector3(CraterLocation.x, CraterLocation.y + CraterOffsetY, CraterLocation.z);
        //StartCoroutine(RainFire());
        RainFirestorm();
    }

    public void ProxySpawnCrater()
    {
        SpawnCrater(CraterLocation);
    }

    public void RainFirestorm()
    {
        //The 2 lines below are useful if Rainstorm is brought back
        //Vector3 TempLoc;
        //TempLoc = new Vector3(CraterLocation.x + Locations[TimesSpawned].x, CraterLocation.y + Locations[TimesSpawned].y, CraterLocation.z);

        
        SpawnFireball(CraterLocation);
    }

    void SpawnFireball(Vector3 SpawnLocation)
    {
        //GameObject Fireball = Instantiate(Fireballs, SpawnLocation, Quaternion.identity);
        GameObject Fireball = ObjectPooler.CentralObjectPool.SpawnFromPool(Fireballs.name, SpawnLocation, Quaternion.identity);
        Fireball.GetComponent<Weapon_Fireball>().SetParentStorm(this);
    }
    void SpawnCrater(Vector3 SpawnLocation)
    {
        //Instantiate(Crater, SpawnLocation, Quaternion.identity);
        ObjectPooler.CentralObjectPool.SpawnFromPool(Crater.name, SpawnLocation, Quaternion.identity);
    }

    IEnumerator RainFire()
    {
        while(TimesSpawned < Locations.Length - 1)
        {
            RainFirestorm();
            TimesSpawned++;

            yield return new WaitForSeconds(0.2f);
        }
        Vector3 LocToSend = CraterLocation - new Vector3(1.5f, 2, CraterLocation.z);
        SpawnCrater(LocToSend);
        TimesSpawned = 0;
    }

}
