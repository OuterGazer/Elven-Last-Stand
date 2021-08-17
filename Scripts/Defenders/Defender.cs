using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defender : MonoBehaviour
{
    [SerializeField] int resourceCost = 20;
    public int ResourceCost => this.resourceCost;

    [SerializeField] GameObject resourceGlitter;
    [SerializeField] int resourceCycles = 6;
    private int cycleCount = 0;

    private ResourceDisplay displayResource;

    private SpriteRenderer unitSprite;

    private AudioSource audioSource;
    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip weaponSFX;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] AudioClip dieSFX;

    private void Start()
    {
        this.displayResource = GameObject.FindObjectOfType<ResourceDisplay>();

        this.unitSprite = this.gameObject.GetComponentInChildren<SpriteRenderer>();

        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefsController.GetMasterSFX();
    }

    public void AddResource(int amount)
    {
        this.cycleCount++;

        if(cycleCount >= this.resourceCycles)
        {
            this.cycleCount = 0;
            this.PlaySFXWithPitch(this.attackSFX);
            Instantiate<GameObject>(this.resourceGlitter, this.gameObject.transform.position, Quaternion.identity);
            this.displayResource.AddResource(amount);
        }        
    }

    public void SetSFXVolume(float inVolume)
    {
        this.audioSource.volume = inVolume;
    }

    public void PlayAttackSFX()
    {
        this.PlaySFXWithPitch(this.attackSFX);
    }

    public void PlayHitSFX()
    {
        this.PlaySFXWithPitch(this.weaponSFX);
    }

    public void PlayHurtSFX()
    {
        this.PlaySFXWithPitch(this.hurtSFX);
    }

    public void PlayDeathSFX()
    {
        this.PlaySFXWithPitch(this.dieSFX);
    }

    private void PlaySFXWithPitch(AudioClip inSFX)
    {
        this.audioSource.pitch = Random.Range(0.9f, 1.1f);
        this.audioSource.PlayOneShot(inSFX);
    }
}
