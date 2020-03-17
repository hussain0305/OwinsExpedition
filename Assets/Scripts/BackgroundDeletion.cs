using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundDeletion : MonoBehaviour {

    private GameObject BackgroundMarker_Bottom;
    
    // Use this for initialization
	void Start () {
        BackgroundMarker_Bottom = GameObject.Find("BackgroundMarker_Bottom");
    }
	
	// Update is called once per frame
	void Update () {
		if(this.transform.position.y <= BackgroundMarker_Bottom.transform.position.y)
        {
            //OPTChange - Destroy(this.gameObject);
            this.gameObject.SetActive(false);
        }
	}

}
