using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestingScript : MonoBehaviour {

    public bool Value;

    void Start () {
        StartCoroutine(TestC());
        if (Value)
        {
            Invoke("Disaa", 5);
        }
    }
	
    IEnumerator TestC()
    {
        while (true)
        {
            if (Value)
            {
                Debug.Log("This is the first one in " + this.gameObject.name);
            }
            else
            {
                Debug.Log("This is the second one in " + this.gameObject.name);
            }

            yield return new WaitForSeconds(1);
        }
    }

    void Disaa()
    {
        StopAllCoroutines();
    }

}
