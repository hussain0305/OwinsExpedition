using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialClassEnemy : MonoBehaviour {

    private int EnemyID;
    private GameObject StopMotionMarker;
    // Use this for initialization
    void Start () {
        StopMotionMarker = GameObject.FindGameObjectWithTag("StopMotionMarker");
        int EnemyID = GetMyType();
    }

    private void OnEnable()
    {
        if (!GetComponent<WorldMovement>())
        {
            gameObject.AddComponent<WorldMovement>();
        }
    }

    // Update is called once per frame
    void Update() {
		if(transform.position.y < StopMotionMarker.transform.position.y && GetComponent<WorldMovement>())
        {
            Destroy(GetComponent<WorldMovement>());
            GetComponent<Rigidbody2D>().velocity=new Vector2(0.0f, 0.0f);
            if (EnemyID == 2)
            {
                this.gameObject.GetComponent<AIPhoenix>().SetReachedStopPosition();
            }
            else if (EnemyID == 4)
            {
                this.gameObject.GetComponent<AIBat>().SetReachedStopPosition();
            }
            else if (EnemyID == 3)
            {
                this.gameObject.GetComponent<AIEyeMonster>().StartFlying();
            }
        }
	}
    public int GetMyType()
    {
        if (this.gameObject.GetComponent<AIGhost>())
        {
            EnemyID = 1;
        }
        else if (this.gameObject.GetComponent<AIPhoenix>())
        {
            EnemyID = 2;
        }
        else if (this.gameObject.GetComponent<AIEyeMonster>())
        {
            EnemyID = 3;
        }
        else if (this.gameObject.GetComponent<AIBat>())
        {
            EnemyID = 4;
        }
        return EnemyID;
    }
}
