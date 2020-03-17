using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAreaDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("ShowRing", 0) == 0)
        {
            Destroy(this.gameObject);
        }
    }
}
