using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindingFilmScript : MonoBehaviour {

    private SpriteRenderer BlindingFilm;

    // Use this for initialization
    void Start () {
        BlindingFilm = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void AdjustAlpha(float Alpha)
    {
        Color newColor = new Color(1, 1, 1, Alpha);
        BlindingFilm.color = newColor;
    }
}
