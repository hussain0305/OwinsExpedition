using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageResizer : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Resize();
    }
	
    void Resize()
    {
        SpriteRenderer SpriteRend = GetComponent<SpriteRenderer>();
        if (SpriteRend == null)
        {
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);

        float width = SpriteRend.sprite.bounds.size.x;
        float height = SpriteRend.sprite.bounds.size.y;


        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        Vector3 xWidth = transform.localScale;
        xWidth.x = worldScreenWidth / width;
        transform.localScale = xWidth;
        Vector3 yHeight = transform.localScale;
        yHeight.y = worldScreenHeight / height;
        transform.localScale = yHeight;

    }

}
