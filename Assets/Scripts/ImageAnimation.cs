using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{
    public float FramesPerSecond;
    public Sprite[] SpriteFrames;

    private int CurrentIndex = 1;
    private int NumberOfImages;
    private float TimePassed = 0;
    private Image ProfileWindowImage;

    // Start is called before the first frame update
    void Start()
    {
        ProfileWindowImage = this.gameObject.GetComponent<Image>();
        NumberOfImages = SpriteFrames.Length;
    }

    // Update is called once per frame
    void Update()
    {
        if (TimePassed > 1 / FramesPerSecond)
        {
            TimePassed = 0;
            ProfileWindowImage.sprite = SpriteFrames[CurrentIndex];
            CurrentIndex++;
            if (CurrentIndex >= NumberOfImages)
            {
                CurrentIndex = 0;
            }
        }
        else
        {
            TimePassed += Time.deltaTime;
        }
    }
}
