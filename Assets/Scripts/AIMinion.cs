using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMinion : MonoBehaviour {

    public float MinionAge = 3;

    private bool Alive;
    private Animator MinionAnimator;

    // Use this for initialization
    public void Awake() {
        Alive = true;
        Invoke("MinionDeathAnim", MinionAge);
        MinionAnimator = GetComponent<Animator>();

    }

    public void OnEnable()
    {
        Alive = true;
        Invoke("MinionDeathAnim", MinionAge);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Obstacle" || collision.gameObject.tag == "BigObstacle" || collision.gameObject.tag == "Supermove" || collision.gameObject.tag == "PlayerWeapon")
        {
            Alive = false;
            MinionDeathAnim();
        }
        
    }

    public void MinionDeathAnim()
    {
        MinionAnimator.SetTrigger("MinionDeath");
    }

    public bool IsAlive()
    {
        return Alive;
    }

    public void MinionDeath()
    {
        //OPTChange - Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
}
