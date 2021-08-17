using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject pauseLabel;
    [SerializeField] GameObject winLabel;
    [SerializeField] float levelCompleteYieldTime = 2.0f;
    [SerializeField] AudioClip winSFX;
    [SerializeField] AudioClip looseSFX;

    [SerializeField] GameObject looseLabel;

    private List<Attacker> currentAliveAttackers;

    private AttackerSpawner attackerSpawner;

    // Start is called before the first frame update
    void Start()
    {
        this.pauseLabel.SetActive(false);
        this.winLabel.SetActive(false);
        this.looseLabel.SetActive(false);

        this.currentAliveAttackers = new List<Attacker>();

        this.attackerSpawner = GameObject.FindObjectOfType<AttackerSpawner>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        this.pauseLabel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        this.pauseLabel.SetActive(false);
    }

    public void AddAttacker(Attacker inAttacker)
    {
        this.currentAliveAttackers.Add(inAttacker);
    }
    public void RemoveAttacker(Attacker inAttacker)
    {
        this.currentAliveAttackers.Remove(inAttacker);

        if (this.currentAliveAttackers.Count <= 0 &&
            !this.attackerSpawner.CanSpawn)
        {
            this.StartCoroutine(this.HandleWinCondition());
        }
    }

    private IEnumerator HandleWinCondition()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.winSFX);
        
        if(this.winLabel != null)
            this.winLabel.SetActive(true);

        Time.timeScale = 0;

        yield return new WaitForSecondsRealtime(this.levelCompleteYieldTime);
    }

    public void StopSpawningAttackers()
    {
        this.attackerSpawner.SetCanSpawn(false);
    }

    //this gets called through a SendMessage from BaseIntegrityDisplay
    public void GameOver()
    {
        this.gameObject.GetComponent<AudioSource>().PlayOneShot(this.looseSFX);
        this.looseLabel.SetActive(true);
        Time.timeScale = 0;
    }
}
