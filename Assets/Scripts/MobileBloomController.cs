using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileBloomController : MonoBehaviour
{
    void Start()
    {
        if(PlayerPrefs.GetInt("BeautifulGraphics", 1) == 0)
        {
            GetComponent<MobileBloom>().enabled = false;
        }
    }
}
