using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supermove_Swords : BaseSupermove
{

    public float SwordSpeed;
    public AudioClip SupermoveSound;
    
    private Transform OriginalTransform;
    private Vector3 SwordOriginalPosition;
    private Quaternion SwordOriginalRotation;

    private GameObject SwordLeft;
    private GameObject SwordMid;
    private GameObject SwordRight;
    private AudioSource SupermoveSoundSource;

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
        //InitializeSounds();
        //SupermoveSoundSource.Play();
        SwordLeft = Instantiate(this.gameObject, GameObject.FindGameObjectWithTag("LeftPosition").transform.position, GameObject.FindGameObjectWithTag("LeftPosition").transform.rotation);
        SwordLeft.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15);
        SwordMid = Instantiate(this.gameObject, GameObject.FindGameObjectWithTag("DefaultPosition").transform.position, GameObject.FindGameObjectWithTag("DefaultPosition").transform.rotation);
        SwordMid.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15);
        SwordRight = Instantiate(this.gameObject, GameObject.FindGameObjectWithTag("RightPosition").transform.position, GameObject.FindGameObjectWithTag("RightPosition").transform.rotation);
        SwordRight.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 15);
        Invoke("DestroySwords", 1);
    }

    public void InitializeSounds()
    {
        if (!SupermoveSoundSource)
        {
            SupermoveSoundSource = gameObject.AddComponent<AudioSource>();
            SupermoveSoundSource.clip = SupermoveSound;
        }
    }

    public void DestroySwords()
    {
        Destroy(SwordLeft);
        Destroy(SwordMid);
        Destroy(SwordRight);
    }

}
