using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    [SerializeField] AudioClip levelMusic;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip sampleSFX;


    private void Awake()
    {
        MusicPlayer[] singletonMusic = GameObject.FindObjectsOfType<MusicPlayer>();
        if (singletonMusic.Length > 1)
        {
            this.gameObject.SetActive(false);
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            GameObject.DontDestroyOnLoad(this.gameObject);
        }

        this.audioSource = this.gameObject.GetComponent<AudioSource>();
        this.audioSource.volume = PlayerPrefsController.GetMasterVolume();
    }

    public void SetVolume(float inVolume)
    {
        this.audioSource.volume = inVolume;
    }

    public void SetSFX(float inVolume)
    {
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            PlayerPrefsController.SetMasterSFX(inVolume);

            this.audioSource.PlayOneShot(this.sampleSFX, inVolume);
        }
        else
        {
            Defender[] defenders = GameObject.FindObjectsOfType<Defender>();
            Attacker[] attackers = GameObject.FindObjectsOfType<Attacker>();

            this.SendMessageToChars(defenders, inVolume);
            this.SendMessageToChars(attackers, inVolume);

            this.audioSource.PlayOneShot(this.sampleSFX, inVolume);
        }
    }

    private void SendMessageToChars(Component[] units, float inVolume)
    {
        if (units.Length > 0)
        {
            for (int i = 0; i < units.Length; i++)
            {
                units[i].SendMessage("SetSFXVolume", inVolume);
            }
        }
    }

    public void PlayLevelMusic()
    {
        this.audioSource.clip = this.levelMusic;
        this.audioSource.Play();
    }

    public void PlayMenuMusic()
    {
        this.audioSource.clip = this.menuMusic;
        this.audioSource.Play();
    }
}
