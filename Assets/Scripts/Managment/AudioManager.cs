using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource sfx;
    [Header("BOSS AUDIO STUFF")]
    public AudioClip threeHitSFX;
    public AudioClip twoHitSFX;
    public AudioClip upwardSFX;
    public AudioClip downwardSFX;
    public AudioClip pillarChargeSFX;
    public AudioClip pillarReleaseSFX;
    public AudioClip laserChargeSFX;
    public AudioClip laserReleaseSFX;
    public AudioClip bossGetHitSFX;
    public AudioClip bossDeath;
    public AudioClip bossWalk;

    [Header("PLAYER AUDIO STUFF")]
    public AudioClip walk;
    public AudioClip jump;
    public AudioClip land;
    public AudioClip lightAttack;
    public AudioClip heavyAttack;
    public AudioClip hurt;
    public AudioClip die;

    [Header("MISC AUDIO STUFF")]
    public AudioClip Purchase;
    public AudioClip Click;

    void Start()
    {
        sfx = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        sfx.PlayOneShot(clip);
    }
}
