using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EurusImageResizer : MonoBehaviour
{
    RectTransform ImageRT;
    // Start is called before the first frame update
    void Start()
    {
        ImageRT = GetComponent<RectTransform>();

        float worldScreenHeight = Screen.height;
        float worldScreenWidth = Screen.width;

        //ImageRT.sizeDelta = new Vector2(Screen.width, Screen.height);
        ImageRT.sizeDelta = new Vector2(0, 0);
    }
}
