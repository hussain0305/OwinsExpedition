using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXScript : MonoBehaviour
{
    public AudioClip DeathSound;

    private Animator EffectAnimator;
    private AudioSource SoundSource;
    // Start is called before the first frame update
    void Awake()
    {
        EffectAnimator = GetComponent<Animator>();
        SoundSource = gameObject.AddComponent<AudioSource>();
        SoundSource.loop = false;
        SoundSource.clip = DeathSound;
        EffectAnimator.SetTrigger("ShootEffect");
    }

    public void OnEnable()
    {
        if(EffectAnimator)
            EffectAnimator.SetTrigger("ShootEffect");
        if(SoundSource)
            SoundSource.Play();
        Invoke("DestroyVFX", 1);
    }

    public void DestroyVFX()
    {
        this.gameObject.SetActive(false);
    }
}
