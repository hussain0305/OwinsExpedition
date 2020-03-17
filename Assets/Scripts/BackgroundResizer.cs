using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundResizer : MonoBehaviour
{
    public float XAdjustmentPercent = 0;
    public float YAdjustmentPercent = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        ResizeSpriteToScreen();
    }

    void ResizeSpriteToScreen()
    {
        SpriteRenderer SR = GetComponent<SpriteRenderer>();
        if (SR == null)
        {
            return;
        }

        transform.localScale = new Vector3(1, 1, 1);

        float Width = SR.sprite.bounds.size.x;
        float Height = SR.sprite.bounds.size.y;

        double worldScreenHeight = Camera.main.orthographicSize * 2.0;
        double worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        float TempX = (float)worldScreenWidth / Width;
        float TempY = (float)worldScreenHeight / Height;

        float ScaleX = TempX + (XAdjustmentPercent/100 * TempX);
        float ScaleY = TempY + (YAdjustmentPercent / 100 * TempY);

        transform.localScale = new Vector2(ScaleX, ScaleY);
    }
}
