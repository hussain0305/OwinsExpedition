using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMovement : MonoBehaviour {
    
    private Rigidbody2D Body;
    //private Camera MainCamera;

    private float WorldMovementSpeed = 2.25f;

    private void Awake()
    {
        Body = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void OnEnable () {
        if(Body)
            Body.velocity = new Vector2(0.0f, -WorldMovementSpeed);
        else
            Body = GetComponent<Rigidbody2D>();

        StartCoroutine(CheckForOutOfScreen());
    }

    public void ModifyMovementSpeed(float NewSpeed)
    {
        Body.velocity = new Vector2(0.0f, -NewSpeed);
    }

    IEnumerator CheckForOutOfScreen()
    {
        while (true)
        {
            if (transform.position.y < -7)
            {
                this.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.75f);
        }
    }
}
