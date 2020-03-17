using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementColoringScript : MonoBehaviour
{
    public Color NormalColor;
    public Color FireColor;
    public Color NormalTailStart;
    public Color NormalTailEnd;
    public Color IceTailStart;
    public Color IceTailEnd;
    public Color FireTailStart;
    public Color FireTailEnd;
    public Color MagicTailStart;
    public Color MagicTailEnd;
    public Sprite Regular;
    public Sprite Ice;
    public Sprite Fire;
    public Sprite Magic;
    public HazeEffectSpawner HazeSpawner;
    public TrailRenderer TailTrail;

    public void SetColorByElement(int Element)
    {
        switch (Element)
        {
            case 0:
                gameObject.GetComponent<SpriteRenderer>().sprite = Regular;
                gameObject.GetComponent<SpriteRenderer>().color = NormalColor;
                TailTrail.startColor = NormalTailStart;
                TailTrail.endColor = NormalTailEnd;
                if (HazeSpawner)
                    HazeSpawner.SetCurrentElement(0);
                break;
            case 1:
                gameObject.GetComponent<SpriteRenderer>().sprite = Ice;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                TailTrail.startColor = IceTailStart;
                TailTrail.endColor = IceTailEnd;
                if (HazeSpawner)
                    HazeSpawner.SetCurrentElement(1);
                break;
            case 2:
                gameObject.GetComponent<SpriteRenderer>().sprite = Fire;
                gameObject.GetComponent<SpriteRenderer>().color = FireColor;
                TailTrail.startColor = FireTailStart;
                TailTrail.endColor = FireTailEnd;
                if (HazeSpawner)
                    HazeSpawner.SetCurrentElement(2);
                break;
            case 3:
                gameObject.GetComponent<SpriteRenderer>().sprite = Magic;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                TailTrail.startColor = MagicTailStart;
                TailTrail.endColor = MagicTailEnd;
                if (HazeSpawner)
                    HazeSpawner.SetCurrentElement(3);
                break;
        }
    }

}
