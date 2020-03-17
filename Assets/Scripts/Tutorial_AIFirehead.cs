using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_AIFirehead : Tutuorial_BaseEnemy
{

    public Animator AnimController;
    public GameObject Fireball;

    private Rigidbody2D Body;
    private TutorialManager TM;

    // Use this for initialization
    new void Awake()
    {
        base.Awake();
        Body = GetComponent<Rigidbody2D>();
        TM = GameObject.FindGameObjectWithTag("WorldManager").GetComponent<TutorialManager>();
    }
    

    public void FireheadKilled()
    {
        if (!TM.FireheadDeathAccountedFor)
        {
            TM.FireheadDeathAccountedFor = true;
            TM.NextInLine();
        }
    }
}
