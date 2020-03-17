using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionScript : MonoBehaviour {

    public float TerminalAgeInSeconds = 1.0f;

    private void OnEnable()
    {
        Invoke("DestroyThyself", TerminalAgeInSeconds);
    }
    

    public void DestroyThyself()
    {
        this.gameObject.SetActive(false);
    }
}
