using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_StabSword : BaseSupermove
{

    public float SwordSpeed;

    private Transform OriginalTransform;
    private Vector3 SwordOriginalPosition;
    private Quaternion SwordOriginalRotation;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        SwordOriginalPosition = transform.position;
        SwordOriginalRotation = transform.rotation;
        //Set Weapon Elemental Damages here. Not exposed in the inspector
        
    }

    public override void PerformSuperMove()
    {
        Instantiate(this.gameObject);
    }
    
}
