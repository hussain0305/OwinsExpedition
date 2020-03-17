using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_PoisonBomb : BaseWeapon {

    // Use this for initialization
    public float PuddleOffsetY = 0.2f;
    public GameObject PoisonPuddle;

    private Vector3 PuddleLocation;

    new void Awake()
    {
        base.Awake();
        ResetOrientation();
    }

    new void OnEnable()
    {
        base.OnEnable();
        ResetOrientation();
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
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(CurrentFingerPosition.x, CurrentFingerPosition.y, Camera.main.nearClipPlane));
        }
        transform.position = new Vector2(transform.position.x, transform.position.y - 0.05f);
    }

    public override void SwipeInitiated(Vector2 SwipeStart)
    {
        SwipeStartAt = SwipeStart;
    }

    public override void SwipeEnded(Vector2 SwipeEnd)
    {
        SwipeEndAt = SwipeEnd;
        PuddleLocation = Camera.main.ScreenToWorldPoint(new Vector3(CurrentFingerPosition.x, CurrentFingerPosition.y, Camera.main.nearClipPlane));
        PuddleLocation = new Vector3(PuddleLocation.x, PuddleLocation.y + PuddleOffsetY, PuddleLocation.z);
    }

    public void SpawnPuddle()
    {
        ObjectPooler.CentralObjectPool.SpawnFromPool(PoisonPuddle.name, PuddleLocation, Quaternion.identity);
    }

    public void CheckForEnemiesInside()
    {

    }

}
