using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSupermove : MonoBehaviour {

    public Sprite SuperIcon;
    //public AudioClip SupermoveSound;

    //public AudioSource SupermoveSoundSource;

    public void Start () {
        //SupermoveSoundSource = gameObject.AddComponent<AudioSource>();
        //SupermoveSoundSource.clip = SupermoveSound;
    }
	
    public void PlaySFX()
    {
        //SupermoveSoundSource.Play();
    }
    
    public virtual void PerformSuperMove()
    {

    }

    public Sprite GetIcon()
    {
        return SuperIcon;
    }
}
