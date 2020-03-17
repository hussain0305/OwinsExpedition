using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Grenade : BaseWeapon {

    // Use this for initialization
    new void Awake() {
        base.Awake();
	}

    new void OnEnable()
    {
        base.OnEnable();
    }
    // Update is called once per frame
    new void Update () {
        TimePassed += Time.deltaTime;
        if (ActionAllowed)
        {
            CurrentFingerPosition = Input.GetTouch(0).position;
            Vector3 diff = Camera.main.ScreenToWorldPoint(new Vector3(CurrentFingerPosition.x, CurrentFingerPosition.y, Camera.main.nearClipPlane));
            transform.position = diff;
        }
    }

    public override void SwipeInitiated(Vector2 SwipeStart)
    {
        SwipeStartAt = SwipeStart;
        transform.localScale = new Vector3(0.06f, 0.06f, 0.06f);
    }

    public override void SwipeEnded(Vector2 SwipeEnd)
    {
        SwipeEndAt = SwipeEnd;
        FireProjectile();
    }

    void FireProjectile()
    {
        transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
    }
}
