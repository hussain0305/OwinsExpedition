using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitPlayerSensor : MonoBehaviour {

    public GameObject RabbitParent;

    private bool AlreadyRegistered;

    private void Awake()
    {
        AlreadyRegistered = false;
    }

    public void SetAlreadyRegistered(bool NewValue)
    {
        AlreadyRegistered = NewValue;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (AlreadyRegistered)
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            //AlreadyRegistered = true;
            RabbitParent.GetComponent<AIRabbit>().PlayerAhead();
        }
    }
}
